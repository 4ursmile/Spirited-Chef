using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card3", menuName = "ScriptableObjects/Card/CardSkill3", order = 3)]
    public class CardSkill3 : CardInforSO
    {
        [SerializeField] private float _waitingtimIncreasePercent = 0.05f;
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.TimePrepareMultiplier *= (1 - _waitingtimIncreasePercent);
        }
        public override string CardDescription => string.Format(LocalizationManager.Localize(_cardDescription), _waitingtimIncreasePercent*100);
    }
}


