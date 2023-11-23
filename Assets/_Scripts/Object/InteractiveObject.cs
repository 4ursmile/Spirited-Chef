using System.Collections;
using System.Collections.Generic;
using Architecture;
using Behaviour;
using Manager;
using UnityEngine;

namespace ObjectS
{
    public class InteractiveObject : MonoBehaviour, IClickable, IInteractable
    {
        [SerializeField] protected SoundManagerSO _soundManagerSO;
        [SerializeField] protected AudioClip _onPerformClip;
        [SerializeField] protected InteractiveObjectSO _interactiveObjectSO;
        [SerializeField] protected InGameUIBridgeSO _inGameUIBridgeSO;
        public SoundManagerSO SoundManager => _soundManagerSO;
        public AudioClip OnPerformClip => _onPerformClip;
        public InteractiveObjectSO InteractiveObjectSO => _interactiveObjectSO;
        public InGameUIBridgeSO UIBridge => _inGameUIBridgeSO;
        protected Vector3 _positionOffset;
        protected bool _requireFocus = false;
        protected bool _available = true;
        public bool Available => _available;
        public void SetAvailble(bool value)
        {
            _available = value;
        }
        protected CharacterControllerS _currentController;
        protected GameController _gameController;
        public GameController GameController => _gameController;
        public Vector3 Position => transform.position + _positionOffset;
        public bool CurrentlyInteracting => _currentController != null;
        public CharacterControllerS CurrentController => _currentController;

        public ObjectType Type => ObjectType.Interactive;
        public string Name => _interactiveObjectSO.Name;
        public string Description => _interactiveObjectSO.Description;

        public virtual bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS == null || controllerS.IsWorking)
            {
                return false;
            }

            if (_available == false)
            {
                _inGameUIBridgeSO.SetWarningText("mess_available", transform.position);
                return false;
            }
            controllerS.SetObjectTive(this, gameController);
            return true;
        }
        public virtual void StopInteract()
        {
            _currentController = null;
        }
        public virtual void PerformInteraction(CharacterControllerS controller, GameController gameController)
        {
            controller.ChangeState(controller.IdleState);
            _currentController = controller;
            _gameController = gameController;
        }
        private void Awake() {
            _positionOffset = _interactiveObjectSO.PositionOffset;
            _requireFocus = _interactiveObjectSO.RequireFocus;
            Init();

        }
        void Start()
        {   
        }
        public virtual void Init()
        {

        }
        public virtual void OnClick()
        {

        }
        public virtual void OnFoodSelected(BaseFoodSO food)
        {

        }
        public void DoDestroy(params GameObject[] gameObjects)
        {
            foreach (var item in gameObjects)
            {
                Destroy(item);
            }
        }
        protected ObjectStateMachine _stateMachine;
        public void ChangeState(IBaseObjectState state)
        {
            _stateMachine.ChangeState(state);
        }
    }
}

