using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class MainUI : UIBase
{
    [Header("��ʯ��ʾ")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float coinCrawlDuration = 0.5f;

    [Header("���ܰ�ť")]
    [SerializeField] private Button minimizeButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button spiritTreeButton;
    private bool isMinimized = false;

    private long currentCoinValue = 0;      // ��ǰ��ʾ�Ľ��
    private Coroutine crawlCoroutine;       // ���ڿ���Э��
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnShow()
    {
        Subscribe("CoinChanged", OnCoinChanged);
        minimizeButton.onClick.AddListener(OnMinimizeClick);
        //storeButton.onClick.AddListener(OnStoreClick);
        //spiritTreeButton.onClick.AddListener(OnSpiritTreeClick);

    }
    // === �¼��ص� ===
    private void OnCoinChanged(object data)
    {
        long newCoinValue = (long)data;

        if (crawlCoroutine != null)
            StopCoroutine(crawlCoroutine);
        Debug.Log($"�յ��¼� CoinChanged: {newCoinValue}");

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

            coinText.text = $"���: {interpolated:D7}"; // 7λ����ǰ�油0
            yield return null;
        }

        coinText.text = $"���: {targetValue:D7}";
        currentCoinValue = targetValue;
        crawlCoroutine = null;
    }
    // === ��ť�߼� ===
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
