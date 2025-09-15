using System;
using UnityEngine;
public enum RarityType
{
    Common,     // 普通
    Rare,       // 稀有
    Epic,       // 史诗
}
public enum SpiritPropety
{
    Grass,
    Water,
    Fire,
    Air,
}
[System.Serializable]
public class SpiritDrop
{
    [Header("基础信息")]
    public string spriteName;
    public GameObject prefab;
    public SpiritPropety propety;

    [Header("数值属性")]
    public float moneyPerSecond;  // 每秒生成的金钱数
    public RarityType rarity;     // 稀有度
    [Range(0f, 1f)] public float dropRate;
}

[CreateAssetMenu(fileName = "SpiritTable", menuName = "Spirit/Drop Table")]
public class SpiritDropTable : ScriptableObject
{
    public SpiritDrop[] drops;
}
