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
        public long lastSaveTime; // �洢���һ�α���ʱ�� (Ticks)

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
            Debug.Log($"ע��Spirit:{data.id}");
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
            Debug.Log($"ɾ��Spirit: {id}");
        }
    }

    public void SaveGame()
    {
        SpiritSaveData saveData = new SpiritSaveData();
        saveData.ownedSpirits.AddRange(ownedSpirits.Values);

        // ����ʱ���
        saveData.lastSaveTime = DateTime.UtcNow.Ticks;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"�浵�ɹ���{savePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("û�д浵������������");
            return;
        }

        string json = File.ReadAllText(savePath);
        SpiritSaveData saveData = JsonUtility.FromJson<SpiritSaveData>(json);

        ownedSpirits.Clear();
        foreach (var s in saveData.ownedSpirits)
        {
            ownedSpirits[s.id] = s;
        }

        Debug.Log($"�����ɹ�: {ownedSpirits.Count} �� Spirit �ָ�");

        //// === ������������ ===
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
        //        Debug.Log($"�������棺+{(long)totalReward} ��ң�����ʱ�� {offlineDuration.TotalMinutes:F1} ����");
        //        // ������Դ��� UI ������ʾ
        //    }
        //}
    }

    private double CalculateProduction(SpiritData spirit, double durationSeconds)
    {
        // ����ʱ��ֵ��ÿ�룩
        double baseValue = spirit.moneyPerSec;

        // Ʒ������
        double rarityMultiplier = spirit.rarity switch
        {
            RarityType.Common => 1.0,
            RarityType.Rare => 2.5,
            RarityType.Epic => 5.0,
            _ => 1.0
        };

        // �������� (��Ϊ1)
        double resonanceMultiplier = 1.0;

        return baseValue * rarityMultiplier * resonanceMultiplier * durationSeconds;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
