using UnityEngine;
using System.Collections;
using System.Linq;
using GlobalGameManager;

public class SpiritProductionManager : MonoBehaviour
{
    [SerializeField] private float tickInterval = 1f; // 每秒结算一次

    private void Start()
    {
        StartCoroutine(ProductionLoop());
    }

    private IEnumerator ProductionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval);

            float totalProduction = 0f;

            foreach (var spirit in GlobalManager.Instance.spiritGameManager.GetAllSpirits())
            {
                totalProduction += CalculateProduction(spirit);
            }

            if (totalProduction > 0)
            {
               GlobalGameManager.GlobalManager.Instance.currencyManager.AddCoins((long)totalProduction);
                Debug.Log($"本轮产出金币: {totalProduction}");
            }
        }
    }

    private float CalculateProduction(SpiritData spirit)
    {
        // === 基础时产值 ===
        float baseValue = spirit.moneyPerSec;

        // === 品级倍率（举例，可以改成枚举映射表） ===
        float rarityMultiplier = spirit.rarity switch
        {
            RarityType.Common => 1f,
            RarityType.Rare => 2.5f,
            RarityType.Epic => 5f,
            _ => 1f
        };

        // === 共鸣乘数（简化版，先返回1） ===
        float resonanceMultiplier = 1f;

        // TODO: 判断地图里同属性精灵/植物数量，动态计算共鸣
        // resonanceMultiplier = 条件成立 ? 1 + (plantCount - 2)/10f : 1f;

        // === 最终时产值 ===
        return baseValue * rarityMultiplier * resonanceMultiplier * tickInterval;
    }
}
