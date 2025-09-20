using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private long coins = 0;

    // 定义一个事件，任何 UI 都可以订阅
    public event Action<long> OnCoinsChanged;

    public long GetCoins() => coins;

    public void AddCoins(long amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins); // 触发事件
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
