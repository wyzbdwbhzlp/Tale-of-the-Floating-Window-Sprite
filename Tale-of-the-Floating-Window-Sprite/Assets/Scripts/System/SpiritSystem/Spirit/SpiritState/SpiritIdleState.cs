using Unity.VisualScripting;
using UnityEngine;

public class SpiritIdleState : SpiritState
{
    Spirit spirit;
    public SpiritIdleState(Spirit _spiritBase, SpiritStateMachine _stateMachine, string _animBoolName) : base(_spiritBase, _stateMachine, _animBoolName)
    {
        spirit = _spiritBase;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = spirit.idleTime;
        spirit.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(spirit.moveState);
        }
    }
}
