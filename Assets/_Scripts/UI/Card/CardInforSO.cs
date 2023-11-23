using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D;

namespace UI
{
    [CreateAssetMenu(fileName = "CardInforSO", menuName = "ScriptableObjects/Card/CardBase", order = 1)]
    public class CardInforSO : ScriptableObject, IComparable, IComparer
    {
        [SerializeField] protected string _cardName;
        [SerializeField] protected string _cardDescription;
        [SerializeField] protected Sprite _cardImage;
        [SerializeField] protected Color _cardImageColor = Color.white;
        [SerializeField] protected CardTier _cardTier;
        [SerializeField] protected int _cardCost;
        [SerializeField] protected int _maxCardLevel;
        [SerializeField] protected SerializedDictionary<string, List<float>> _cardParams;
        [SerializeField] protected string _spriteName;
        [SerializeField] protected bool infiniteLevel = false;
        protected int _cardLevel;
        public CardTier CardTier => _cardTier;
        public bool InfiniteLevel => infiniteLevel;
        public Sprite CardImage => _cardImage;
        public int CardCost => _cardCost;
        public int CardLevel => _cardLevel;
        public int CardMaxLevel => _maxCardLevel;
        public int CurrentCardAvailable => _maxCardLevel - _cardLevel;
        public virtual string CardName => LocalizationManager.Localize(_cardName);
        public virtual string CardDescription => LocalizationManager.Localize(_cardDescription);
        public bool IsReachMaxLevel => _cardLevel >= _maxCardLevel;
        public Color CardImageColor => _cardImageColor;
        protected void LevelUp()
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

