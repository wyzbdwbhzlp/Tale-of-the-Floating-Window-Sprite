using UnityEngine;
using Game.UI;

public class GameEntry : MonoBehaviour
{
    void Start()
    {
        // 打开主界面
        GlobalGameManager.GlobalManager.Instance.uiManager.Show<MainUI>();

        // 主动推一次初始金币
        long current = GlobalGameManager.GlobalManager.Instance.currencyManager.GetCoins();
        UIBase.Publish("CoinChanged", current);

        // 测试加金币
        GlobalGameManager.GlobalManager.Instance.currencyManager.AddCoins(100);
    }
}
