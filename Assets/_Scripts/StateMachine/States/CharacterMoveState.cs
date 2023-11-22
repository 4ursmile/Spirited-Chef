using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Behaviour
{
    [CreateAssetMenu(fileName = "CharacterMoveState", menuName = "ScriptableObjects/States/CharacterMove")]
    public class CharacterMoveStateSO : CharacterBaseStateSO
    {

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _animator.speed = _animtionSpeed;
        }
        public override void OnStateExit()
        {
            base.OnStateExit();
            _characterController.StopMoving();
        }
        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (_characterController.DestinationReached)
            {
                _characterController.ChangeState(_characterController.IdleState);
                _characterController.OnDestinationReachedCallBacks?.Invoke();
            }
        }
    }
}

