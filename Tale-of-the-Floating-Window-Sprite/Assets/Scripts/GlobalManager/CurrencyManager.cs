using UnityEngine;
using Game.UI;
public class CurrencyManager : MonoBehaviour
{
   

    [SerializeField]private long coins = 0;

    public long GetCoins() => coins;

    public void AddCoins(long amount)
    {
        coins += amount;
        UIBase.Publish("CoinChanged", coins);
        Debug.Log($"AddCoins: {amount}, total = {coins}");

    }

    public bool SpendCoins(long amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UIBase.Publish("CoinChanged", coins);
            return true;
        }
        return false;
    }
}
