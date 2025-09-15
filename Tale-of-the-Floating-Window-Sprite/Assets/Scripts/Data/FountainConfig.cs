using UnityEngine;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FountainConfig", menuName = "Game/Fountain Config")]
public class FountainConfig : ScriptableObject
{
    [Header("��������")]

    //[LabelText("С��������ʱ�� (��)")]
    [SerializeField]
    private float spiritGenerationTimer;

    //[LabelText("���鴢������")]
    [SerializeField]
    private float spiritStoreLimitation;

    // ����ֻ������
    public float SpiritGenerationTimer => spiritGenerationTimer;
    public float SpiritStoreLimitation => spiritStoreLimitation;
}
