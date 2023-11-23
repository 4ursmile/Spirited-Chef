using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Behaviour;
using Architecture;
using ObjectS;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UI;
namespace Manager
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableLayer;
        [SerializeField] private InGameUIBridgeSO _bridgeUI;
        [SerializeField] private SoundManagerSO _soundManagerSO;
        [SerializeField] private AudioClip _onSelectClip;
        [SerializeField] private AudioClip _onObecjtiveClip;
        [SerializeField] private AudioClip _onGroundClip;
        private InputManager _inputManager;
        private CameraController _cameraController;
        private Camera _camera;
        private CharacterControllerS _currentSelected;
        private InteractiveObject _currentInteractable;
        public bool IsFocused {get; private set;}
        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();

        }
        private void Start() {

            _cameraController = _inputManager.CameraController;
            _camera = Camera.main;
            _currentSelected = null;
        }
        private void OnEnable() {
            _inputManager.OnClickAction += OnClickHandle;   
        }
        private void OnDisable() {
            _inputManager.OnClickAction -= OnClickHandle;
        }
        public void OnClickHandle(Vector2 position)
        {
            if (IsFocused) return;
            Ray ray = _camera.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, math.INFINITY, _clickableLayer))
            {
                IClickable clickableObject = hit.collider.GetComponent<IClickable>();
                if (clickableObject == null) return;
                if (clickableObject.Type == ObjectType.Character)
                {
                    CharacterControllerS selectable = hit.collider.GetComponent<CharacterControllerS>();

                    SelectCharacter(selectable);
                }
                else if (clickableObject.Type == ObjectType.Ground)
                {
                    if (_currentSelected != null)
                    {
                        if (_currentSelected.IsWorking) return;
                        _currentSelected.RemoveCurrentTarget();
                        _currentSelected.SetDestination(hit.point);
                        _soundManagerSO.Play(_onGroundClip);
                    }
                } else if (clickableObject.Type == ObjectType.Interactive)
                {
                    InteractiveObject interactiveObject = hit.collider.GetComponent<InteractiveObject>();
                    _currentSelected?.RemoveCurrentTarget();
                    ClickToInteractiveObject(interactiveObject);
                    interactiveObject.Interact(_currentSelected, this);
                    _soundManagerSO.Play(_onSelectClip);
                }
            }
        }

        private void SelectCharacter(CharacterControllerS selectable)
        {
            if (selectable.IsWorking) return;
            if (selectable == _currentSelected)
            {
                selectable.Deselect();
                _bridgeUI.UnSetTarget();
                _currentSelected = null;
                _cameraController.Unfollow();
            }
            else
            {
                if (_currentSelected != null)
                {
                    _currentSelected.Deselect();
                    _bridgeUI.UnSetTarget();
                }
                _bridgeUI.SetFollowTarget(selectable.transform);
                _cameraController.FollowTarget(selectable.transform);
                selectable.Select();
                _currentSelected = selectable;
                _soundManagerSO.Play(_onObecjtiveClip);
            }
        }
        private void ClickToInteractiveObject(InteractiveObject interactive)
        {
            if (_currentInteractable == interactive)
            {
                _currentInteractable = null;
                _bridgeUI.UnSetInforUITarget();
            }
            else
            {
                if (_currentInteractable != null)
                {
                    _bridgeUI.UnSetInforUITarget();
                }
                _currentInteractable = interactive;
                if (_currentSelected != null) 
                return;
                _bridgeUI.SetInforUITarget(interactive.transform, interactive.Name, interactive.Description);
            }
        }
        public void RequireFocussing(CharacterControllerS characterControllerS)
        {
            if (IsFocused)
                return;
            SelectCharacter(characterControllerS);
            IsFocused = true;
        }
        public void UnFocus()
        {
            IsFocused = false;
        }
    }
}
