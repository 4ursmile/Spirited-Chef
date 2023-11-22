using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using StateMachine;
using System;
using Character;
using ObjectS;
using DG.Tweening;
using Manager;
using UnityEngine.Animations.Rigging;
namespace Behaviour
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterControllerS : MonoBehaviour, ISelectable, IClickable
    {
        [SerializeField] private CharacterCustomSO _characterCustomSO;
        private CharacterCustomsie _characterCustomsie;
        [SerializeField] private Transform _holdTransform;
        [SerializeField] private SpriteFollow _spriteFollowPrefab;
        private RigBuilder _rigBuilder;

        public string Name => _characterCustomSO.Name;
        public string Description => _characterCustomSO.Description;
        private TStateMachine _stateMachine;
        public ObjectType Type => ObjectType.Character;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        public Vector3 Position => transform.position;
        public Animator Animator => _animator;
        [Header("States")]
        [SerializeField] private CharacterBaseStateSO _idleState;
        [SerializeField] private CharacterBaseStateSO _moveState;
        [SerializeField] private CharacterBaseStateSO _workState;
        public CharacterBaseStateSO IdleState {get; private set;}
        public CharacterBaseStateSO MoveState {get; private set;}
        public CharacterBaseStateSO WorkState {get; private set;}
        private GameController _gameController;
        public float Speed {
            get => _navMeshAgent.speed;
            set {
                _navMeshAgent.speed = value;
                _maxVelocity = value;
            }
        }
        public float AngularSpeed {
            get => _navMeshAgent.angularSpeed;
            set {
                _navMeshAgent.angularSpeed = value;
            }

        }
        private float _maxVelocity;
        private float _minVelocity = 0;
        [Header("Animation")]
        [SerializeField] private string _speedParameterName = "Speed";
        private int _speedParameterHash;
        public int SpeedParameterHash => _speedParameterHash;

        [Header("Destination")]
        [SerializeField] private float _destinationReachedThreshold = 0.5f;
        private bool _isworking = false; 
        public bool DestinationReached => (Destination - transform.position).sqrMagnitude <= _destinationReachedThreshold*_destinationReachedThreshold;
        public float DestinationReachedThreshold => _destinationReachedThreshold;
        public Vector3 Destination => _navMeshAgent.destination;
        public Action OnDestinationReachedCallBacks {get; private set;}
        public InteractiveObject CurrentTarget {get; private set;}
        public RequireObject CurrentHoldObject {get; private set;}
        public RequireObjectType CurrentHoldObjectType => CurrentHoldObject?.RequireObjectType ?? RequireObjectType.None;
        public BaseFoodSO CurrentHoldFood {get; private set;}
        public void OnDestinationReachedCallBack(Action callBack)
        {
            OnDestinationReachedCallBacks = callBack;
        }
        private void Awake()
        {
            _rigBuilder = GetComponent<RigBuilder>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _characterCustomsie = GetComponent<CharacterCustomsie>();
            _maxVelocity = _navMeshAgent.speed;
            IdleState = _idleState.Copy().Init(this);
            MoveState = _moveState.Copy().Init(this);
            WorkState = _workState.Copy().Init(this);
            _stateMachine = new TStateMachine(IdleState);
        }
        private void Start() {
            UnHoldSomething();
            _stateMachine.OnStateEnter();
            HashAnimationParameters();
        }
        private void HashAnimationParameters()
        {
            _speedParameterHash = Animator.StringToHash(_speedParameterName);
        }
        public void SetDestination(Vector3 destination)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);
            _stateMachine.ChangeState(MoveState);
            OnDestinationReachedCallBack(() => {
                _stateMachine.ChangeState(IdleState);
            });
        }
        public void StopMoving()
        {
            _navMeshAgent.isStopped = true;
        }
        public void SetObjectTive(InteractiveObject target, GameController gameController)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(target.Position);
            _stateMachine.ChangeState(MoveState);
            CurrentTarget = target;
            OnDestinationReachedCallBack(() => {
                RotateToTarget(target.transform.position);
                target.PerformInteraction(gameController);
            });
        }
        public void RemoveCurrentTarget()
        {
            CurrentTarget?.StopInteract();
            CurrentTarget = null;
        }
        private void RotateToTarget(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.DORotate(new Vector3(0, angle, 0), 0.3f);
        }
        public void ResumeMoving()
        {
            _navMeshAgent.isStopped = false;
            _stateMachine.ChangeState(MoveState);
        }
        public void ChangeState(CharacterBaseStateSO newState)
        {
            _stateMachine.ChangeState(newState);
        }
        public void Deselect()
        {
            _characterCustomsie.Deselect();
        }

        public void OnClick()
        {
        }

        public void Select()
        {
            _characterCustomsie.Select();
        }
        public void SetHoldSomething(RequireObject requireObject)
        {
            
            HoldSomething();
            CurrentHoldObject = Instantiate(requireObject, _holdTransform);
            CurrentHoldObject.transform.localPosition = Vector3.zero;
        }
        public void RemoveHoldSomething()
        {
            CurrentHoldObject = null;
            UnHoldSomething();
        }
        private SpriteFollow _spriteFollow;
        public void SetHoldFood(BaseFoodSO baseFoodSO, Action<BaseFoodSO> oldFoodCallBack = null)
        {
            HoldSomething();
            if (_spriteFollow != null)
            {
                oldFoodCallBack?.Invoke(CurrentHoldFood);
                _spriteFollow.SetTarget(_holdTransform, baseFoodSO);
                CurrentHoldFood.Set(baseFoodSO);
                return;
            }
            _spriteFollow = Instantiate(_spriteFollowPrefab);
            _spriteFollow.SetTarget(_holdTransform, baseFoodSO);
            CurrentHoldFood = baseFoodSO;
        }
        public void RemoveHoldFood()
        {
            CurrentHoldFood = null;
            UnHoldSomething();
        }
        private void HoldSomething()
        {
            _rigBuilder.layers[0].active = true;
        }
        private void UnHoldSomething()
        {
            _rigBuilder.layers[0].active = false;
        }
        // Start is called before the first frame update


        // Update is called once per frame
        void Update()
        {
              _stateMachine.FrameUpdate();
              UpdateAnimator();

        }
        private void UpdateAnimator()
        {

            _animator.SetFloat(_speedParameterHash, _navMeshAgent.velocity.magnitude.MinMaxScale(_minVelocity, _maxVelocity));
        }
        private void FixedUpdate()
        {
            _stateMachine.PhysicUpdate();
        }
        private void OnTriggerEnter(Collider other)
        {
            _stateMachine.OnTriggerEnter(other);
        }
        private void OnCollisionEnter(Collision other)
        {
            _stateMachine.OnCollisionEnter(other);
        }
        public bool IsWorking => _isworking;
        public void StarWorking()
        {
            _isworking = true;
            _animator.SetBool("IsWorking", true);
        }
        public void EndWorking()
        {
            _isworking = false;
            _animator.SetBool("IsWorking", false);
        }

    }
}
