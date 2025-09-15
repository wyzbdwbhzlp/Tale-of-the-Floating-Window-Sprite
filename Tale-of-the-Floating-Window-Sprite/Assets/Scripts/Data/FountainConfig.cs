using UnityEngine;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FountainConfig", menuName = "Game/Fountain Config")]
public class FountainConfig : ScriptableObject
{
    [Header("精灵生成")]

    //[LabelText("小精灵生成时间 (秒)")]
    [SerializeField]
    private float spiritGenerationTimer;

    //[LabelText("精灵储存上限")]
    [SerializeField]
    private float spiritStoreLimitation;

    // 对外只读访问
    public float SpiritGenerationTimer => spiritGenerationTimer;
    public float SpiritStoreLimitation => spiritStoreLimitation;
}
