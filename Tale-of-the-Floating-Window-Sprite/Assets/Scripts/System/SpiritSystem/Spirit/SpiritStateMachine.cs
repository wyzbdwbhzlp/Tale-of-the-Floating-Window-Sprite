using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritStateMachine
{
    public SpiritState currentState { get; private set; }
    public void Initialize(SpiritState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    public void ChangeState(SpiritState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
