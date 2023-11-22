using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Behaviour
{
    public abstract class CharacterBaseStateSO : ScriptableObject, IBaseState
    {
        [SerializeField] protected float _animtionSpeed = 1f;
        [SerializeField] protected float _animtionTransitionSpeed = 0.1f;
        public float AnimationSpeed => _animtionSpeed;
        public void SetAnimationSpeed(float speed)
        {
            _animtionSpeed = speed;
            _animator.speed = _animtionSpeed;
        }
        protected CharacterControllerS _characterController;
        protected Animator _animator;
        protected Transform _transform;
        public CharacterBaseStateSO Copy()
        {
            return Instantiate(this);
        }
        public virtual CharacterBaseStateSO Init(CharacterControllerS characterController)
        {
            _characterController = characterController;
            _animator = _characterController.Animator;
            _transform = _characterController.transform;
            return this;

        }

        public virtual void FrameUpdate()
        {
        }

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {
        }

        public virtual void PhysicUpdate()
        {
        }
        public virtual void OnTriggerEnter(Collider other) {
            
        }
        public virtual void OnCollisionEnter(Collision other) {
            
        }
        public virtual void Assign(System.Action Callbacks)
        {
        }
    }
}

