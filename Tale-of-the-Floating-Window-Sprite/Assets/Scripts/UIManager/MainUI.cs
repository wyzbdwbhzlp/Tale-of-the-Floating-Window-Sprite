using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class MainUI : UIBase
{
    [Header("宝石显示")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float coinCrawlDuration = 0.5f;

    [Header("功能按钮")]
    [SerializeField] private Button minimizeButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button spiritTreeButton;
    private bool isMinimized = false;

    private long currentCoinValue = 0;      // 当前显示的金币
    private Coroutine crawlCoroutine;       // 用于控制协程
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnShow()
    {
        Subscribe("CoinChanged", OnCoinChanged);
        minimizeButton.onClick.AddListener(OnMinimizeClick);
        //storeButton.onClick.AddListener(OnStoreClick);
        //spiritTreeButton.onClick.AddListener(OnSpiritTreeClick);

    }
    // === 事件回调 ===
    private void OnCoinChanged(object data)
    {
        long newCoinValue = (long)data;

        if (crawlCoroutine != null)
            StopCoroutine(crawlCoroutine);
        Debug.Log($"收到事件 CoinChanged: {newCoinValue}");

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

            coinText.text = $"金币: {interpolated:D7}"; // 7位数，前面补0
            yield return null;
        }

        coinText.text = $"金币: {targetValue:D7}";
        currentCoinValue = targetValue;
        crawlCoroutine = null;
    }
    // === 按钮逻辑 ===
    private void OnMinimizeClick()
    {
        isMinimized = !isMinimized;
        coinText.gameObject.SetActive(!isMinimized);
        storeButton.gameObject.SetActive(!isMinimized);
        spiritTreeButton.gameObject.SetActive(!isMinimized);
    }

    //private void OnStoreClick()
    //{
    //    UIManager.Instance.Show<StoreUI>();
    //}

    //private void OnSpiritTreeClick()
    //{
    //    UIManager.Instance.Show<SpiritTreeUI>();
    //}
}
