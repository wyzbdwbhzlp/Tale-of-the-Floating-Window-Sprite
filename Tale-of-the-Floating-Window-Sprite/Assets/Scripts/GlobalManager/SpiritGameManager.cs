using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SpiritData
{
    public string id;                 // 名称或类型，比如 "FireSpirit"
    public RarityType rarity;
    public SpiritPropety propety;
    public float moneyPerSec;
    public string prefabPath;         // Resources 路径
}

[System.Serializable]
public class SpiritSaveData
{
    public List<SpiritData> ownedSpirits = new List<SpiritData>();
    public long lastSaveTime; // 存档时间戳
}

public class SpiritGameManager : MonoBehaviour
{
    // 一个 key 对应多个 Spirit
    private Dictionary<string, List<SpiritData>> ownedSpirits = new Dictionary<string, List<SpiritData>>();
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "spirits.json");
        //LoadGame();
    }

    /// <summary>
    /// 注册精灵（支持同名）
    /// </summary>
    public void RegisterSpirit(SpiritData data)
    {
        if (!ownedSpirits.ContainsKey(data.id))
        {
            ownedSpirits[data.id] = new List<SpiritData>();
        }
        ownedSpirits[data.id].Add(data);
        Debug.Log($"注册 Spirit: {data.id}, 当前数量: {ownedSpirits[data.id].Count}");
    }

    /// <summary>
    /// 获取某种精灵的所有实例
    /// </summary>
    public IEnumerable<SpiritData> GetSpirits(string id)
    {
        return ownedSpirits.ContainsKey(id) ? ownedSpirits[id] : new List<SpiritData>();
    }

    /// <summary>
    /// 获取所有精灵（遍历字典里的所有 List）
    /// </summary>
    public IEnumerable<SpiritData> GetAllSpirits()
    {
        foreach (var list in ownedSpirits.Values)
        {
            foreach (var spirit in list)
            {
                yield return spirit;
            }
        }
    }

    /// <summary>
    /// 删除某个具体的精灵（按索引或对象删除）
    /// </summary>
    public void RemoveSpirit(string id, SpiritData target)
    {
        if (ownedSpirits.ContainsKey(id))
        {
            ownedSpirits[id].Remove(target);
            if (ownedSpirits[id].Count == 0)
                ownedSpirits.Remove(id);

            Debug.Log($"删除 Spirit: {id}, 剩余数量: {(ownedSpirits.ContainsKey(id) ? ownedSpirits[id].Count : 0)}");
        }
    }

    /// <summary>
    /// 存档
    /// </summary>
    public void SaveGame()
    {
        SpiritSaveData saveData = new SpiritSaveData();

        foreach (var list in ownedSpirits.Values)
        {
            saveData.ownedSpirits.AddRange(list);
        }

        saveData.lastSaveTime = DateTime.UtcNow.Ticks;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"存档成功：{savePath}");
    }

    /// <summary>
    /// 读档
    /// </summary>
    /// 
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
            RegisterSpirit(s);

           
            GameObject prefab = Resources.Load<GameObject>(s.prefabPath);
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab);
                var spirit = obj.GetComponent<Spirit>();
                if (spirit != null)
                {
                    spirit.Init(s.moneyPerSec, s.rarity, s.propety);
                }
            }
            else
            {
                Debug.LogWarning($"❌ 无法加载精灵 prefab: {s.prefabPath}");
            }
        }

        Debug.Log($"读档成功: {saveData.ownedSpirits.Count} 个 Spirit 恢复");
    }
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("精灵存档已删除！");
        }
        else
        {
            Debug.Log("没有找到存档文件，跳过删除。");
        }

        ownedSpirits.Clear();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
