using System.Collections;
using GlobalGameManager;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null; // 等待一帧，让所有 UI 的 Awake 执行完
        GlobalManager.Instance.uiManager.Show<MainUI>();
        GlobalManager.Instance.currencyManager.AddCoins(100);
    }
}
