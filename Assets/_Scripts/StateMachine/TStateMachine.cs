using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace StateMachine
{
    public interface IStateMachine
    {
        public IBaseState CurrentState { get;}
        public void ChangeState(IBaseState newState);
    }
    public class TStateMachine : IStateMachine
    {
        public IBaseState CurrentState { get; private set; }
        public TStateMachine(IBaseState initialState, bool immediate = false)
        {
            CurrentState = initialState;
            if (immediate)
            {
                CurrentState.OnStateEnter();
            }
        }
        public void OnStateExit() => CurrentState.OnStateExit();
        public void OnStateEnter() => CurrentState.OnStateEnter();
        public void FrameUpdate() => CurrentState.FrameUpdate();
        public void PhysicUpdate() => CurrentState.PhysicUpdate();
        public void OnTriggerEnter(Collider other) => CurrentState.OnTriggerEnter(other);
        public void OnCollisionEnter(Collision other) => CurrentState.OnCollisionEnter(other);
        public void ChangeState(IBaseState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }
            CurrentState.OnStateExit();
            CurrentState = newState;
            CurrentState.OnStateEnter();
        }
        public async void ChangeState(IBaseState newState, float delay)
        {
            if (CurrentState == newState)
            {
                return;
            }
            CurrentState.OnStateExit();
            await Task.Delay((int)(delay * 1000));
            CurrentState = newState;
            CurrentState.OnStateEnter();
        }
        public async void ChangeState(IBaseState newState, float delay, Action onBeforeChange, Action onAfterChange = null)
        {
            if (CurrentState == newState)
            {
                return;
            }
            CurrentState.OnStateExit();
            onBeforeChange?.Invoke();
            await Task.Delay((int)(delay * 1000));
            CurrentState = newState;
            CurrentState.OnStateEnter();
            onAfterChange?.Invoke();
        }
        public void Assign(Action Callbacks)
        {
            CurrentState.Assign(Callbacks);
        }
    }
}