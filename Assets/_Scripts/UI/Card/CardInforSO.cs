using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D;

namespace UI
{
    [CreateAssetMenu(fileName = "CardInforSO", menuName = "ScriptableObjects/Card/CardBase", order = 1)]
    public class CardInforSO : ScriptableObject, IComparable, IComparer
    {
        [SerializeField] private string _cardName;
        [SerializeField] private string _cardDescription;
        [SerializeField] private Sprite _cardImage;
        [SerializeField] private Color _cardImageColor = Color.white;
        [SerializeField] private CardTier _cardTier;
        [SerializeField] private int _cardCost;
        [SerializeField] private int _maxCardLevel;
        [SerializeField] private SerializedDictionary<string, List<float>> _cardParams;
        [SerializeField] private string _spriteName;
        [SerializeField] private bool infiniteLevel = false;
        private int _cardLevel;
        public CardTier CardTier => _cardTier;
        public bool InfiniteLevel => infiniteLevel;
        private string CardDescriptionBuilder(object[] cardParams)
        {
            string cardDes = _cardDescription;  
            var strRes = String.Format(cardDes, cardParams);
            return strRes;
        }
        private object[] cardParamsBuidler()
        {
            object[] cardParams = new object[_cardParams.Count];
            foreach (var (item, index) in _cardParams.Enumerate())
            {
                cardParams[index] = item.Value[_cardLevel];
            }
            return cardParams;
        }
        public string GetCardDescription => CardDescriptionBuilder(cardParamsBuidler());
        public float GetCardParam(string key) => _cardParams[key][_cardLevel];
        public int GetCardParamInt(string key) => Mathf.RoundToInt(_cardParams[key][_cardLevel]);
        public string CardDescription => _cardDescription;
        public Sprite CardImage => _cardImage;
        public int CardCost => _cardCost;
        public int CardLevel => _cardLevel;
        public int CardMaxLevel => _maxCardLevel;
        public int CurrentCardAvailable => _maxCardLevel - _cardLevel;
        public string CardName => _cardName;
        public bool IsReachMaxLevel => _cardLevel >= _maxCardLevel;
        public Color CardImageColor => _cardImageColor;
        private void LevelUp()
        {
            if (_cardLevel < _maxCardLevel)
            {
                _cardLevel++;
            }
        }
        public virtual void Init()
        {
            _cardLevel = 0;
        }
        public Sprite GetSprite(SpriteAtlas atlas = null)
        {
            if (atlas == null)
                return _cardImage;
            return atlas.GetSprite(_spriteName);
        }
        public virtual void ApplyEffect(CardManagerSO cardManagerSO = null)
        {
            LevelUp();
        }

        public int CompareTo(object obj)
        {
            var a = obj as CardInforSO;
            if (a == null)
                return 1;
            return _cardName.CompareTo(a._cardName);
        }

        public int Compare(object x, object y)
        {
            var a = x as CardInforSO;
            var b = y as CardInforSO;
            if (a == null || b == null)
                return 1;
            return a._cardName.CompareTo(b._cardName);
        }
        public override string ToString()
        {
            return _cardName;
        }
        public override bool Equals(object obj)
        {
            var a = obj as CardInforSO;
            if (a == null)
                return false;
            return _cardName.Equals(a._cardName);
        }
        public override int GetHashCode()
        {
            return _cardName.GetHashCode() + _cardTier.GetHashCode() + _cardCost.GetHashCode() + _maxCardLevel.GetHashCode();
        }   
    }
}

