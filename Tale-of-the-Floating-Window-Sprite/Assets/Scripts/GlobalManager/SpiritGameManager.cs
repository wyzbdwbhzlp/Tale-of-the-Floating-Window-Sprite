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

    }
public class SpiritGameManager : MonoBehaviour
{
  public static SpiritGameManager Instance { get; private set; }
  private Dictionary<string, SpiritData> ownedSpirits = new Dictionary<string, SpiritData>();
    private string savePath;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "spirits.json");
        LoadGame(); // Æô¶¯Ê±³¢ÊÔ
    }
    public void RegisterSpirit(SpiritData data)
    {
        if (!ownedSpirits.ContainsKey(data.id))
        {
            ownedSpirits[data.id] = data;
            Debug.Log($"×¢²áSpirit:{data.id}");
        }
    }
    public SpiritData GetSpirit(string id)
    {
        return ownedSpirits.ContainsKey(id)?ownedSpirits[id]:null;
    }
    public IEnumerable<SpiritData> GetAllSpirits()
    {
        return ownedSpirits.Values;
    }
    public void RemoveSpirit(string id)
    {
        if (ownedSpirits.ContainsKey(id))
        {
            ownedSpirits.Remove(id);
            Debug.Log($"É¾³ýSpirit: {id}");
        }
    }
    public void SaveGame()
    {
        SpiritSaveData saveData = new SpiritSaveData();
        saveData.ownedSpirits.AddRange(ownedSpirits.Values);
        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(savePath, json);
        Debug.Log($"´æµµ³É¹¦£º{savePath}");
    }
    // ¶Áµµ
    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Ã»ÓÐ´æµµ£¬Ìø¹ý¶Áµµ¡£");
            return;
        }

        string json = File.ReadAllText(savePath);
        SpiritSaveData saveData = JsonUtility.FromJson<SpiritSaveData>(json);

        ownedSpirits.Clear();
        foreach (var s in saveData.ownedSpirits)
        {
            ownedSpirits[s.id] = s;
        }

        Debug.Log($"¶Áµµ³É¹¦: {ownedSpirits.Count} ¸ö Spirit »Ö¸´");
    }
    private void OnApplicationQuit()
    {
        SpiritGameManager.Instance.SaveGame();
    }
}

