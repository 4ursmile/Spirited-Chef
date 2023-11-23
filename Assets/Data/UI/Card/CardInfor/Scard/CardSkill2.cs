using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card2", menuName = "ScriptableObjects/Card/CardSkill2", order = 2)]
    public class CardSkill2 : CardInforSO
    {
        [SerializeField] private float _waitingtimIncreasePercent = 0.05f;
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.TimeToWaittMultiplier *= (1 + _waitingtimIncreasePercent);
        }
        public override string CardDescription => string.Format(LocalizationManager.Localize(_cardDescription), _waitingtimIncreasePercent*100);
    }
}


