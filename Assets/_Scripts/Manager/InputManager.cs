using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ET = UnityEngine.InputSystem.EnhancedTouch ;
using IS = UnityEngine.InputSystem;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] InputUIBridgeSO _inputUIBridgeSO;
        private Camera _camera;
        private CameraController _cameraController;
        public CameraController CameraController => _cameraController;
        public Action<Vector2> OnClickAction;
        private InputControl _inputControl;
        private CustomTouch _customTouch;
        private Vector2 _keyboardMove;
        private void Awake() {
            _inputControl = new InputControl();
            _customTouch = new CustomTouch();
        }
        private void Start() {
            _camera = Camera.main;
            _cameraController = _camera.GetComponent<CameraController>();
        }
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ActiveInputCallbacks();
        }



        private void OnDisable()
        {

            EnhancedTouchSupport.Disable();
            DeActiveInputCallbacks();

        }
        // Start is called before the first frame update
        // Update is called once per frame
        void Update()
        {
            if (ET.Touch.activeTouches.Count > 0)
            {
                ControlHandle();
                _cameraController.IsControlled = true;
            } else
            {
                _cameraController.IsControlled = false;
            }           
        }
        void ControlHandle()
        {
            ET.Touch touch = ET.Touch.activeTouches[0];
            if (touch.phase == IS.TouchPhase.Began)
                _inputUIBridgeSO.TouchInScreen(touch.screenPosition);
            if (ET.Touch.activeFingers.Count == 1)
            {
                ClickAction(touch);
            }
            else if (ET.Touch.activeFingers.Count == 2)
            {
                ZoomCamera(touch, ET.Touch.activeTouches[1]);
            }
        }
        private void ClickAction(ET.Touch touch)
        {
            if (touch.phase == IS.TouchPhase.Began)
                OnClickAction?.Invoke(touch.screenPosition);
            else if (touch.phase == IS.TouchPhase.Moved)
                MoveCamera(touch);
        }
        private void ZoomCamera(ET.Touch touch1, ET.Touch touch2)
        {
            Vector2 touch1Delta = touch1.delta.normalized;
            Vector2 touch2Delta = touch2.delta.normalized;
            Vector2 center = (touch1.screenPosition + touch2.screenPosition) / 2;
            float dotA = Vector2.Dot(touch1Delta, touch2Delta);
            if (dotA < -0.1)
            {
                Vector2 deltaCurrent = touch1.screenPosition - center;
                Vector2 deltaStart = touch1.startScreenPosition - center;
                float deltaMag = deltaCurrent.sqrMagnitude > deltaStart.sqrMagnitude ? 1 : -1;
                _cameraController.ZoomCamera(deltaMag);
            }

            
        }
        private void MoveCamera(ET.Touch touch)
        {
            Vector2 touchDelta = touch.delta.normalized;
            _cameraController.MoveCamera(touchDelta);
        }


        private void ActiveInputCallbacks()
        {
            _inputControl.Enable();
            _inputControl.Action.Zoom.performed += ZoomCamera;
        }
        private void DeActiveInputCallbacks()
        {
            _inputControl.Action.Zoom.performed -= ZoomCamera;
            _inputControl.Disable();

        }
        private void ZoomCamera(InputAction.CallbackContext ctx)
        {
            Vector2 delta = ctx.ReadValue<Vector2>();
            _cameraController.ZoomCamera(delta.normalized.y);
        }
    }
}

