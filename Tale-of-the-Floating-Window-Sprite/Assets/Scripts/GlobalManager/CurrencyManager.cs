using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private long coins = 0;

    // ����һ���¼����κ� UI �����Զ���
    public event Action<long> OnCoinsChanged;

    public long GetCoins() => coins;

    public void AddCoins(long amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins); // �����¼�
        Debug.Log($"AddCoins: {amount}, total = {coins}");
    }

    public bool SpendCoins(long amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            OnCoinsChanged?.Invoke(coins);
            return true;
        }
        return false;
    }
}
