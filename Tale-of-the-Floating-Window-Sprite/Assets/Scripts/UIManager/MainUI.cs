using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GlobalGameManager;
using Game.UI;

public class MainUI : UIBase
{
    [Header("宝石显示")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float coinCrawlDuration = 0.5f;

    [Header("功能按钮")]
    [SerializeField] private Button minimizeButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button spiritTreeButton;
    [SerializeField] private Button deleteButton;

    private bool isMinimized = false;
    private long currentCoinValue = 0;
    private Coroutine crawlCoroutine;

    protected override void OnShow()
    {
        // 订阅金币事件
        var currency = GlobalManager.Instance.currencyManager;
        currency.OnCoinsChanged += OnCoinChanged;

        // 初始化显示
        OnCoinChanged(currency.GetCoins());

        // 按钮绑定
        minimizeButton.onClick.AddListener(OnMinimizeClick);
        storeButton.onClick.AddListener(OnStoreClick);
        deleteButton.onClick.AddListener(OnDeleteClick);
    }

    protected override void OnHide()
    {
        var currency = GlobalManager.Instance.currencyManager;
        if (currency != null)
            currency.OnCoinsChanged -= OnCoinChanged;

        minimizeButton.onClick.RemoveAllListeners();
        storeButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
    }

    private void OnCoinChanged(long newCoinValue)
    {
        if (crawlCoroutine != null)
            StopCoroutine(crawlCoroutine);

        crawlCoroutine = StartCoroutine(CrawlToTarget(newCoinValue));
    }

    private IEnumerator CrawlToTarget(long targetValue)
    {
        long startValue = currentCoinValue;
        float timer = 0f;

        while (timer < coinCrawlDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / coinCrawlDuration);

            long interpolated = (long)Mathf.Lerp(startValue, targetValue, progress);
            coinText.text = $"{interpolated:D7}";
            yield return null;
        }

        coinText.text = $"{targetValue:D7}";
        currentCoinValue = targetValue;
        crawlCoroutine = null;
    }

    private void OnMinimizeClick()
    {
        isMinimized = !isMinimized;
        coinText.gameObject.SetActive(!isMinimized);
        storeButton.gameObject.SetActive(!isMinimized);
        spiritTreeButton.gameObject.SetActive(!isMinimized);
    }

    private void OnStoreClick()
    {
        GlobalManager.Instance.uiManager.Hide<MainUI>();
        //GlobalManager.Instance.uiManager.Show<StoreUI>();
    }

    private void OnDeleteClick()
    {
        GlobalManager.Instance.spiritGameManager.DeleteSave();
        Debug.Log("已清除存档");
    }
}
