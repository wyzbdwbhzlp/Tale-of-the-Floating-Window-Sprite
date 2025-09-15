using UnityEngine;
using Game.UI;

public class GameEntry : MonoBehaviour
{
    void Start()
    {
        // ��������
        GlobalGameManager.GlobalManager.Instance.uiManager.Show<MainUI>();

        // ������һ�γ�ʼ���
        long current = GlobalGameManager.GlobalManager.Instance.currencyManager.GetCoins();
        UIBase.Publish("CoinChanged", current);

        // ���Լӽ��
        GlobalGameManager.GlobalManager.Instance.currencyManager.AddCoins(100);
    }
}
