using System.Collections;
using GlobalGameManager;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null; // �ȴ�һ֡�������� UI �� Awake ִ����
        GlobalManager.Instance.uiManager.Show<MainUI>();
        GlobalManager.Instance.currencyManager.AddCoins(100);
    }
}
