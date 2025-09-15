
using UnityEngine;

public class Spirit : Entity
{
    public SpiritIdleState idleState { get; private set; }
    public SpiritMoveState moveState { get; private set; }
    [Header("æ´¡È Ù–‘")]
    [SerializeField]private float moneyPerSecond;
    [SerializeField]private RarityType rarity;
    [SerializeField]private SpiritPropety spiritPropety;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Awake()
    {
        base.Awake();
        idleState = new SpiritIdleState(this, stateMachine, "Idle");
        moveState = new SpiritMoveState(this, stateMachine, "Move");
       
    }
    
    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    public override  void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    public void Init(float money, RarityType type,SpiritPropety propety)
    {
        moneyPerSecond = money;
        rarity = type;
        spiritPropety = propety;
        
    }
}
