using UnityEngine;

public class SpiritState 
{
    protected SpiritStateMachine stateMachine;
    public Spirit spiritBase;
    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimer;
    public SpiritState(Spirit _spiritBase, SpiritStateMachine _stateMachine, string _animBoolName)
    {
        this.spiritBase = _spiritBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Enter()
    {
        triggerCalled = false;
        spiritBase.anim.SetBool(animBoolName, true);
    }
    public virtual void Exit()
    {
        spiritBase.anim.SetBool(animBoolName, false);

    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
