using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card4", menuName = "ScriptableObjects/Card/CardSkill4", order = 4)]
    public class CardSkill4 : CardInforSO
    {
        [SerializeField] private float _waitingtimIncreasePercent = 0.05f;
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.IncomeReceiveMultiplier *= (1 + _waitingtimIncreasePercent);
        }
        public override string CardDescription => string.Format(LocalizationManager.Localize(_cardDescription), _waitingtimIncreasePercent*100);
    }
}


