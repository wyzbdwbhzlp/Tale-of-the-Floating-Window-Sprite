using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

    [System.Serializable]
    public class SpiritData {
        public string id;
        public RarityType rarity;
        public SpiritPropety propety;
        public float moneyPerSec;
        public string prefabPath;
    }
    [System.Serializable]
    public class SpiritSaveData
    {
        public List<SpiritData> ownedSpirits = new List<SpiritData>();
        public long lastSaveTime; // 存储最后一次保存时间 (Ticks)

}
public class SpiritGameManager : MonoBehaviour
{
    
    private Dictionary<string, SpiritData> ownedSpirits = new Dictionary<string, SpiritData>();
    private string savePath;

    private void Awake()
    {
       

        savePath = Path.Combine(Application.persistentDataPath, "spirits.json");
        LoadGame();
    }

    public void RegisterSpirit(SpiritData data)
    {
        if (!ownedSpirits.ContainsKey(data.id))
        {
            ownedSpirits[data.id] = data;
            Debug.Log($"注册Spirit:{data.id}");
        }
    }

    public SpiritData GetSpirit(string id) =>
        ownedSpirits.ContainsKey(id) ? ownedSpirits[id] : null;

    public IEnumerable<SpiritData> GetAllSpirits() => ownedSpirits.Values;

    public void RemoveSpirit(string id)
    {
        if (ownedSpirits.ContainsKey(id))
        {
            ownedSpirits.Remove(id);
            Debug.Log($"删除Spirit: {id}");
        }
    }

    public void SaveGame()
    {
        SpiritSaveData saveData = new SpiritSaveData();
        saveData.ownedSpirits.AddRange(ownedSpirits.Values);

        // 保存时间戳
        saveData.lastSaveTime = DateTime.UtcNow.Ticks;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"存档成功：{savePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("没有存档，跳过读档。");
            return;
        }

        string json = File.ReadAllText(savePath);
        SpiritSaveData saveData = JsonUtility.FromJson<SpiritSaveData>(json);

        ownedSpirits.Clear();
        foreach (var s in saveData.ownedSpirits)
        {
            ownedSpirits[s.id] = s;
        }

        Debug.Log($"读档成功: {ownedSpirits.Count} 个 Spirit 恢复");

        //// === 计算离线收益 ===
        //if (saveData.lastSaveTime > 0)
        //{
        //    DateTime lastTime = new DateTime(saveData.lastSaveTime, DateTimeKind.Utc);
        //    TimeSpan offlineDuration = DateTime.UtcNow - lastTime;

        //    double offlineSeconds = offlineDuration.TotalSeconds;
        //    double totalReward = 0;

        //    foreach (var spirit in ownedSpirits.Values)
        //    {
        //        totalReward += CalculateProduction(spirit, offlineSeconds);
        //    }

        //    if (totalReward > 0)
        //    {
        //        GlobalGameManager.GlobalManager.Instance.currencyManager.AddCoins((long)totalReward);
        //        Debug.Log($"离线收益：+{(long)totalReward} 金币，离线时长 {offlineDuration.TotalMinutes:F1} 分钟");
        //        // 这里可以触发 UI 弹窗提示
        //    }
        //}
    }

    private double CalculateProduction(SpiritData spirit, double durationSeconds)
    {
        // 基础时产值（每秒）
        double baseValue = spirit.moneyPerSec;

        // 品级倍率
        double rarityMultiplier = spirit.rarity switch
        {
            RarityType.Common => 1.0,
            RarityType.Rare => 2.5,
            RarityType.Epic => 5.0,
            _ => 1.0
        };

        // 共鸣乘数 (简化为1)
        double resonanceMultiplier = 1.0;

        return baseValue * rarityMultiplier * resonanceMultiplier * durationSeconds;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
