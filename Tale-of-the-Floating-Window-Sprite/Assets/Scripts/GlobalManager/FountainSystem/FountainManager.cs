using GlobalGameManager;
using UnityEngine;

public class FountainManager : MonoBehaviour
{
    [SerializeField] private SpiritDropTable spiritDropTable;

    [Header("生成区域 (挂载多个 BoxCollider2D)")]
    public BoxCollider2D[] spawnAreas;

    [Header("精灵生成数据")]
    public float spiritStoreLimitation;
    public float spiritGenerationTimer = 10f;

    private float timer = 0f;
    private int currentSpirits = 0;

    private void Update()
    {
        if (currentSpirits >= spiritStoreLimitation) return;

        timer += Time.deltaTime;
        if (timer >= spiritGenerationTimer)
        {
            SpawnRandomSpirit();
            timer = 0f;
        }
    }

    private void SpawnRandomSpirit()
    {
        SpiritDrop drop = GetRandomDrop();
        if (drop == null || spawnAreas.Length == 0) return;

        // 随机选一个区域
        var area = spawnAreas[Random.Range(0, spawnAreas.Length)];

        // 取 BoxCollider2D 的范围
        Bounds bounds = area.bounds;
        Vector3 randomPos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0f // 2D 游戏保持 Z=0
        );

        // 实例化
        GameObject obj = Instantiate(drop.prefab, randomPos, Quaternion.identity);
        var spirit = obj.GetComponent<Spirit>();
        if (spirit != null)
        {
            spirit.Init(drop.moneyPerSecond, drop.rarity, drop.propety);

            GlobalManager.Instance.spiritGameManager.RegisterSpirit(new SpiritData
            {
                id = $"{drop.spriteName}_{System.Guid.NewGuid():N}",
                rarity = drop.rarity,
                propety = drop.propety,
                moneyPerSec = drop.moneyPerSecond,
                prefabPath = $"Spirits/{drop.propety}/{drop.prefab.name.Replace("(Clone)", "")}"
            });
        }

        currentSpirits++;
        Debug.Log($"生成精灵: {drop.spriteName}, 位置: {randomPos}");
    }

    private SpiritDrop GetRandomDrop()
    {
        if (spiritDropTable.drops.Length == 0) return null;

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
