using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "Card1", menuName = "ScriptableObjects/Card/CardSkill1", order = 1)]
    public class CardSkill1LightSO : CardInforSO
    {
        [SerializeField] private float _speedIncreasePercent;
        public override void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            base.ApplyEffect(cardManagerSO);
            UniversalObjectInstance.Instance.IncreaseCharacterSpeed(_speedIncreasePercent);
        }
        public override string CardDescription => string.Format(LocalizationManager.Localize(_cardDescription), _speedIncreasePercent*100);
    }
}


