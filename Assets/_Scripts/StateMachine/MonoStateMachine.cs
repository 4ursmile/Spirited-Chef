using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoStateMachine : MonoBehaviour, IStateMachine
{
    [HideInInspector] public IBaseState CurrentState { get; private set; }
    public void Initialize(IBaseState initialState)
    {
        CurrentState = initialState;
        CurrentState.OnStateEnter();
    }
    public void ChangeState(IBaseState newState)
    {
        CurrentState.OnStateExit();
        MoveToNewState(newState);
    }

    private void MoveToNewState(IBaseState newState)
    {
        CurrentState = newState;
        CurrentState.OnStateEnter();
    }

    public void OnDestroy()
    {
          CurrentState.OnStateExit();
    }
    public void ChangeState(IBaseState newState, float duration)
    {
        CurrentState.OnStateExit();
        Invoke(nameof(MoveToNewState), duration);
    }
    public void ChangeState(IBaseState newState, float duration, Action callbalck)
    {
        CurrentState.OnStateExit();
        callbalck?.Invoke();
        Invoke(nameof(MoveToNewState), duration);
    }
    public void ChangeState(IBaseState newState, Action callbalck)
    {
        CurrentState.OnStateExit();
        callbalck?.Invoke();
    }


}
