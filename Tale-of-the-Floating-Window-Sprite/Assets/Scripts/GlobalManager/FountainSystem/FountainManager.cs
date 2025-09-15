using Unity.VisualScripting;
using UnityEngine;

public class FountainManager : MonoBehaviour
{
    
    [SerializeField] private SpiritDropTable spiritDropTable;//精灵属性表

    [Header("生成点")]
    [SerializeField] private Transform spawnPoint;
    [Header("精灵生成数据")]
    public float spiritStoreLimitation;
    public float spiritGenerationTimer;

   [SerializeField] private float timer = 0f;//精灵生成计时器
    private int currentSpirits = 0;//当前的精灵数

    private void Update()
    {
        if (currentSpirits >= spiritStoreLimitation)//如果精灵已达上限就返回退出
            return;

        timer += Time.deltaTime;
        if (timer >= spiritGenerationTimer)
        {
            Debug.Log("达到生成时间");
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
                prefabPath = $"Spirits/{drop.prefab.name}" // 建议 prefab 放在 Resources/Spirits 下
            });
            timer = 0;
        }
        currentSpirits++;
        Debug.Log($"生成精灵: {drop.spriteName}, 每秒金币: {drop.moneyPerSecond}, 稀有度: {drop.rarity}");
    
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
