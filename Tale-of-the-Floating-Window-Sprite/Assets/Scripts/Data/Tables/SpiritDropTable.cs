using System;
using UnityEngine;
public enum RarityType
{
    Common,     // ��ͨ
    Rare,       // ϡ��
    Epic,       // ʷʫ
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
    [Header("������Ϣ")]
    public string spriteName;
    public GameObject prefab;
    public SpiritPropety propety;

    [Header("��ֵ����")]
    public float moneyPerSecond;  // ÿ�����ɵĽ�Ǯ��
    public RarityType rarity;     // ϡ�ж�
    [Range(0f, 1f)] public float dropRate;
}

[CreateAssetMenu(fileName = "SpiritTable", menuName = "Spirit/Drop Table")]
public class SpiritDropTable : ScriptableObject
{
    public SpiritDrop[] drops;
}
