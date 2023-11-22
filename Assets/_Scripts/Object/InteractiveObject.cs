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
        protected Vector3 _positionOffset;
        protected bool _requireFocus = false;
        protected bool _available = true;
        public bool Available => _available;
        protected CharacterControllerS _currentController;
        public Vector3 Position => transform.position + _positionOffset;
        public bool CurrentlyInteracting => _currentController != null;
        public CharacterControllerS CurrentController => _currentController;

        public ObjectType Type => ObjectType.Interactive;
        public string Name => _interactiveObjectSO.Name;
        public string Description => _interactiveObjectSO.Description;

        public virtual bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS == null)
            {
                return false;
            }
            if ( _currentController != null)
            {
                _inGameUIBridgeSO.SetWarningText("mess_already", transform.position);
                return false;
            }

            if (_available == false)
            {
                _inGameUIBridgeSO.SetWarningText("mess_available", transform.position);
                return false;
            }
            _currentController = controllerS;
            _currentController.SetObjectTive(this, gameController);
            return true;
        }
        public virtual void StopInteract()
        {
            _currentController = null;
        }
        public virtual void PerformInteraction(GameController gameController)
        {
            _currentController.ChangeState(_currentController.IdleState);
            if (_requireFocus && gameController.IsFocused)
                return;
            if (_currentController == null)
                return;   
            Debug.Log("Performing interaction with " + gameObject.name);
        }
        private void Awake() {
            _positionOffset = _interactiveObjectSO.PositionOffset;
            _requireFocus = _interactiveObjectSO.RequireFocus;
        }
        void Start()
        {   
            Init();
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
    }
}

