using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card5", menuName = "ScriptableObjects/Card/CardSkill5", order = 5)]
    public class CardSkill5 : CardInforSO
    {
        [SerializeField] private float _waitingtimIncreasePercent = 0.05f;
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.IncomeMultiplier *= (1 + _waitingtimIncreasePercent);
        }
        public override string CardDescription => string.Format(LocalizationManager.Localize(_cardDescription), _waitingtimIncreasePercent*100);
    }
}


