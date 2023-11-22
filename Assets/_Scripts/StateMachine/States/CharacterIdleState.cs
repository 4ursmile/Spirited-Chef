using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Behaviour
{
    [CreateAssetMenu(fileName = "CharacterIdleState", menuName = "ScriptableObjects/States/CharacterIdle")]
    public class CharacterIdleStateSO : CharacterBaseStateSO
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }
        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}

