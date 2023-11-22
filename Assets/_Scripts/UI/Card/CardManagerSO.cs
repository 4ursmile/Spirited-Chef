using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace UI 
{
    [CreateAssetMenu(fileName = "CardManagerSO", menuName = "ScriptableObjects/UI/CardManagerSO", order = 1)]
    public class CardManagerSO : ScriptableObject
    {
        private CardManager _cardManager;

        private int _numberOfCardSelectInQueue = 0;
        [SerializeField] private int _startCardRollNumber = 3;
        private int _currentCardRollNumber;
        public int CurrentCardRollNumber => _currentCardRollNumber;
        public int MaxCardRollNumber => _startCardRollNumber;
        public void Init(CardManager cardManager)
        {
            _cardManager = cardManager;
            _randomBehaviour = new RandomBehaviour(_cardTierRate);
            CardPool = Instantiate(_cardPoolSO);
            _currentCardRollNumber = _startCardRollNumber;
            _numberOfCardSelectInQueue = 0;
        }
        public void Init()
        {

        }
        public void Add2SelectQueue(int amount = 1)
        {
            _numberOfCardSelectInQueue += amount;
        }
        public void Add2RollNumber(int amount = 1)
        {
            _currentCardRollNumber += amount;
            if (_currentCardRollNumber > _startCardRollNumber)
            {
                _currentCardRollNumber = _startCardRollNumber;
            }
            if (_currentCardRollNumber <= 0)
            {
                _currentCardRollNumber = 0;
            }
        }
        public void ResetRollNumber()
        {
            _currentCardRollNumber = _startCardRollNumber;
        }
        public bool CanSelectCard => _numberOfCardSelectInQueue > 0;

        [SerializeField] private List<int> _cardTierRate;
        [Header("Card Config field")]
        [SerializeField] private CardPoolSO _cardPoolSO;
        public CardPoolSO CardPool {get; private set;}

        private RandomBehaviour _randomBehaviour;
        public RandomBehaviour RandomBehaviour => _randomBehaviour;

        public void UpdateCardTierRate(List<int> cardTierRate)
        {
            _randomBehaviour.Update(cardTierRate);
        }
        public CardTier GetCardTier()
        {
            int index = _randomBehaviour.GetRandomIndex();
            return (CardTier)index;
        }
        public List<CardInforSO> GetCards(CardTier tier, int number = 3)
        {
            return CardPool.GetCardInforSOsRandom(tier, number);
        }
        public void CardSelected(CardInforSO cardInfor)
        {
            cardInfor.ApplyEffect(this);
            CardPool.CardSelected(cardInfor);
        }
    }
}