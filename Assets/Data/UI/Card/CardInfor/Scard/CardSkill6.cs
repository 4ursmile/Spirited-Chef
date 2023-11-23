using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card6", menuName = "ScriptableObjects/Card/CardSkill6", order = 6)]
    public class CardSkill6 : CardInforSO
    {
        [SerializeField] private float _waitingtimIncreasePercent = 0.05f;
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.OutcomeMultiplier *= (1 - _waitingtimIncreasePercent);
        }
        public override string CardDescription => string.Format(LocalizationManager.Localize(_cardDescription), _waitingtimIncreasePercent*100);
    }
}


