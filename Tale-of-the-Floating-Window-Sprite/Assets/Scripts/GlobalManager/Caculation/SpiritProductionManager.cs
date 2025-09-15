using UnityEngine;
using System.Collections;
using System.Linq;
using GlobalGameManager;

public class SpiritProductionManager : MonoBehaviour
{
    [SerializeField] private float tickInterval = 1f; // ÿ�����һ��

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
                Debug.Log($"���ֲ������: {totalProduction}");
            }
        }
    }

    private float CalculateProduction(SpiritData spirit)
    {
        // === ����ʱ��ֵ ===
        float baseValue = spirit.moneyPerSec;

        // === Ʒ�����ʣ����������Ըĳ�ö��ӳ��� ===
        float rarityMultiplier = spirit.rarity switch
        {
            RarityType.Common => 1f,
            RarityType.Rare => 2.5f,
            RarityType.Epic => 5f,
            _ => 1f
        };

        // === �����������򻯰棬�ȷ���1�� ===
        float resonanceMultiplier = 1f;

        // TODO: �жϵ�ͼ��ͬ���Ծ���/ֲ����������̬���㹲��
        // resonanceMultiplier = �������� ? 1 + (plantCount - 2)/10f : 1f;

        // === ����ʱ��ֵ ===
        return baseValue * rarityMultiplier * resonanceMultiplier * tickInterval;
    }
}
