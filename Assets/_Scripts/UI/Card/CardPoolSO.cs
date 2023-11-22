using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "CardPoolSO", menuName = "ScriptableObjects/UI/CardPoolSO")]
    public class CardPoolSO : ScriptableObject
    {
        [SerializeField] private List<CardInforSO> _cardSilverInforSOs;
        [SerializeField] private List<CardInforSO> _cardGoldInforSOs;
        [SerializeField] private List<CardInforSO> _cardDiamondInforSOs;
        public List<CardInforSO> GetCardInforSOs(CardTier tier)
        {
            switch (tier)
            {
                case CardTier.Common:
                    return _cardSilverInforSOs;
                case CardTier.Epic:
                    return _cardGoldInforSOs;
                case CardTier.Legendary:
                    return _cardDiamondInforSOs;
                default:
                    return null;
            }
        }
        public List<CardInforSO> GetCardInforSOsRandom(CardTier tier, int number = 3)
        {
            switch (tier)
            {
                case CardTier.Common:
                    return _cardSilverInforSOs.GetRandomList(number);
                case CardTier.Epic:
                    return _cardGoldInforSOs.GetRandomList(number);
                case CardTier.Legendary:
                    return _cardDiamondInforSOs.GetRandomList(number);
                default:
                    return null;
            }   
        }
        public List<CardInforSO> GetCardHuge(int number)
        {
            List<CardInforSO> cardInforSOs = new List<CardInforSO>(_cardSilverInforSOs);
            cardInforSOs.AddRange(_cardGoldInforSOs);
            cardInforSOs.AddRange(_cardDiamondInforSOs);
            return cardInforSOs.GetRandomList(number, false);
        }
        public void CardSelected(CardInforSO cardInfor)
        {
            var tier = cardInfor.CardTier;
            if (!cardInfor.IsReachMaxLevel || cardInfor.InfiniteLevel)
            {
                return;
            }
            switch (tier)
            {
                case CardTier.Common:
                    _cardSilverInforSOs.Remove(cardInfor);
                    break;
                case CardTier.Epic:
                    _cardGoldInforSOs.Remove(cardInfor);
                    break;
                case CardTier.Legendary:
                    _cardDiamondInforSOs.Remove(cardInfor);
                    break;
                default:
                    break;
            }
        }
    }
}
