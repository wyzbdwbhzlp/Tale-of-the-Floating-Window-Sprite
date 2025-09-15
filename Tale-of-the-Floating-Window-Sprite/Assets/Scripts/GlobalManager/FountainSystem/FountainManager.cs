using Unity.VisualScripting;
using UnityEngine;

public class FountainManager : MonoBehaviour
{
    
    [SerializeField] private SpiritDropTable spiritDropTable;//�������Ա�

    [Header("���ɵ�")]
    [SerializeField] private Transform spawnPoint;
    [Header("������������")]
    public float spiritStoreLimitation;
    public float spiritGenerationTimer;

   [SerializeField] private float timer = 0f;//�������ɼ�ʱ��
    private int currentSpirits = 0;//��ǰ�ľ�����

    private void Update()
    {
        if (currentSpirits >= spiritStoreLimitation)//��������Ѵ����޾ͷ����˳�
            return;

        timer += Time.deltaTime;
        if (timer >= spiritGenerationTimer)
        {
            Debug.Log("�ﵽ����ʱ��");
            SpawnRandomSpirit();
            timer = 0;
        }
    }
    private void SpawnRandomSpirit()
    {
        SpiritDrop drop = GetRandomDrop();
        if (drop == null) return;
        GameObject obj = Instantiate(drop.prefab, spawnPoint.position, Quaternion.identity);
        var spirit=obj.GetComponent<Spirit>();
        if (spirit != null)
        {
            spirit.Init(drop.moneyPerSecond, drop.rarity,drop.propety);
            SpiritGameManager.Instance.RegisterSpirit(new SpiritData
            {
                id = drop.spriteName,
                rarity = drop.rarity,
                propety = drop.propety,
                moneyPerSec = drop.moneyPerSecond,
                prefabPath = $"Spirits/{drop.prefab.name}" // ���� prefab ���� Resources/Spirits ��
            });
            timer = 0;
        }
        currentSpirits++;
        Debug.Log($"���ɾ���: {drop.spriteName}, ÿ����: {drop.moneyPerSecond}, ϡ�ж�: {drop.rarity}");
    
}
    private SpiritDrop GetRandomDrop()
    {
        if(spiritDropTable.drops.Length==0)return null;
        float totalRate = 0f;
        foreach (var drop in spiritDropTable.drops)
            totalRate += drop.dropRate;

        float roll = Random.Range(0f, totalRate);
        float current = 0f;
        foreach (var drop in spiritDropTable.drops)
        {
            current += drop.dropRate;
            if (roll <= current) return drop;
        }

        return spiritDropTable.drops[0];

    }
}
