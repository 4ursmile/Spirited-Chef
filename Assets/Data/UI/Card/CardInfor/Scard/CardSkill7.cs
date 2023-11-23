using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card7", menuName = "ScriptableObjects/Card/CardSkill7", order = 7)]
    public class CardSkill7 : CardInforSO
    {
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.ActiveWaitingObject();
        }
    }
}


