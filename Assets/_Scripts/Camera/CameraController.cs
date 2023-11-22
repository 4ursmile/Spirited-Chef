using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _pitch;
    [SerializeField] private Vector2 _followBoundingSize;
    [Header("Camera Limit")]
    [SerializeField] private Vector2 _mMX;
    [SerializeField] private Vector2 _mMZ;
    private Vector2 _currentFollowBoundingSize;
    private Vector2 _currentScreenInWordSize;
    private float _standardZoom;
    private CameraMode _cameraMode;
    private Camera _camera;
    public Camera Camera => _camera;
    private float _currentZoom = 10f;
    private Transform followTarget;
    private bool _isControlled;
    public bool IsControlled { get => _isControlled; set => _isControlled = value; }
    private void Awake() {
        _camera = GetComponent<Camera>();
        _cameraMode = CameraMode.Free;
        _currentZoom = _standardZoom = _camera.fieldOfView;

    }
    // Start is called before the first frame update
    void Start()
    {
        IsControlled = false;
        _currentFollowBoundingSize = _followBoundingSize;
        _offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraMode == CameraMode.Follow)
        {
            FollowHandle();
        }


    }
    void FollowHandle()
    {
        if (_isControlled) return;
        if (followTarget == null) return;
        Vector3 targetPosition = followTarget.position;
        Vector3 currentPosition = transform.position - _offset;
        Vector3 direction = targetPosition - currentPosition;
        if (direction.x > _currentFollowBoundingSize.x)
        {
            currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x - _currentFollowBoundingSize.x, _speed * Time.deltaTime);
        }
        else if (direction.x < -_currentFollowBoundingSize.x )
        {
            currentPosition.x = Mathf.Lerp(currentPosition.x, targetPosition.x + _currentFollowBoundingSize.x, _speed * Time.deltaTime);
        }
        if (direction.z > _currentFollowBoundingSize.y)
        {
            currentPosition.z = Mathf.Lerp(currentPosition.z, targetPosition.z - _currentFollowBoundingSize.y, _speed * Time.deltaTime);
        }
        else if (direction.z < -_currentFollowBoundingSize.y)
        {
            currentPosition.z = Mathf.Lerp(currentPosition.z, targetPosition.z + _currentFollowBoundingSize.y, _speed * Time.deltaTime);
        }
        currentPosition = Utils.Clamp2D(currentPosition, _mMX, _mMZ);               
        transform.position = currentPosition + _offset;
    }
    public void MoveCamera(Vector2 deltaPos)
    {
        Vector3 delta = deltaPos.ToVec3();
        Vector3 currentPos = transform.position - _offset;
        Vector3 newPos = currentPos + delta * _speed * Time.deltaTime;
        newPos = Utils.Clamp2D(newPos, _mMX, _mMZ);
        transform.position = newPos + _offset;
    }
    public void FollowTarget(Transform target)
    {
        followTarget = target;
        _cameraMode = CameraMode.Follow;
    }
    public void ZoomCamera(float zoomAmount)
    {
        _currentZoom -= zoomAmount * _zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
        _currentFollowBoundingSize = _followBoundingSize * (_currentZoom/_standardZoom);
        _camera.fieldOfView = _currentZoom;
    }
    public void Unfollow()
    {
        _cameraMode = CameraMode.Free;
    }
}
public enum CameraMode
{
    Free,
    Follow
}
