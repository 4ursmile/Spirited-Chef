using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.InputSystem;
using IS=UnityEngine.InputSystem;
using ET=UnityEngine.InputSystem.EnhancedTouch;
using System;
public class CustomTouch 
{
    private InputAction _inputAction;
    private Vector2 _startPos;
    public Vector2 startPos => _startPos;
    private IS.TouchPhase _touchPhase;
    public IS.TouchPhase phase => _touchPhase;
    private Vector2 _currentPos;
    public Vector2 screenPosition => _currentPos;
    public Vector2 delta => _currentPos - _startPos;
    public bool triggered => _inputAction.triggered;
    public Action<CustomTouch> OnClickAction;
    public void BidingInputAction(InputAction inputAction)
    {
        _inputAction = inputAction;
        _inputAction.started += ctx => OnTouchStart(ctx);
        _inputAction.performed += ctx => OnTouchPerform(ctx);
        _inputAction.canceled += ctx => OnTouchCanceled(ctx);
    }
    public void UnBidingInputAction()
    {
        
        _inputAction.started -= ctx => OnTouchStart(ctx);
        _inputAction.performed -= ctx => OnTouchPerform(ctx);
        _inputAction.canceled -= ctx => OnTouchCanceled(ctx);
        _inputAction = null;
    }
    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        _startPos = ctx.ReadValue<Vector2>();
        _touchPhase = IS.TouchPhase.Began;
        _currentPos = _startPos;
        OnClickAction?.Invoke(this);
    }
    public void OnTouchPerform(InputAction.CallbackContext ctx)
    {
        _currentPos = ctx.ReadValue<Vector2>();
        _touchPhase = IS.TouchPhase.Moved;
        OnClickAction?.Invoke(this);
    }
    public void OnTouchCanceled(InputAction.CallbackContext ctx)
    {
        _touchPhase = IS.TouchPhase.Canceled;
        _currentPos = _startPos = Vector2.zero;
        OnClickAction?.Invoke(this);
        _touchPhase = IS.TouchPhase.None;
    }
}
