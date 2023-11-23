using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Manager;
using UnityEngine;

namespace ObjectS
{
    public interface IBaseObjectState
    {
        public void OnEnter();
        public void OnExit();
        public bool OnInteract(CharacterControllerS controllerS, GameController gameController);
        public void OnPerform(CharacterControllerS controllerS, GameController gameController);
        public void Init(InteractiveObject interactiveObject);
    }
    public class ObjectStateMachine
    {
        private IBaseObjectState _currentState;
        public ObjectStateMachine(IBaseObjectState state)
        {
            _currentState = state;
            _currentState.OnEnter();
        }
        public void ChangeState(IBaseObjectState state)
        {
            _currentState.OnExit();
            _currentState = state;
            _currentState.OnEnter();
        }
        public bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            return _currentState.OnInteract(controllerS, gameController);
        }
        public void Perform(CharacterControllerS controllerS, GameController gameController)
        {
            _currentState.OnPerform(controllerS, gameController);
        }

    }
}