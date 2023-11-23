using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace ObjectS
{
    
    public class BaseFoodSO : ScriptableObject, IEquatable<BaseFoodSO>, IComparable
    {
        [Header("Food Infor")]
        [SerializeField] string _nameID;
        [SerializeField] string _descriptionID;
        [SerializeField] Sprite _icon;
        [SerializeField] float _iconScale = 1;
        [SerializeField] int _price;
        [SerializeField] int _level = 0;
        [SerializeField] int _calorie;
        [SerializeField] float _timeToPrepare;
        [SerializeField] bool _isUnlocked;
        [field: SerializeField] public bool NeedToWash { get; set; }
        public string NameID => _nameID;
        public string DescriptionID => _descriptionID;
        public Sprite Icon => _icon;
        public float IconScale => _iconScale;
        public virtual int Price => _price;
        public int Calorie => _calorie;
        public int Level => _level;
        public float TimeToPrepare => _timeToPrepare;
        public bool IsUnlocked => _isUnlocked;
        public List<float> GetEmbedding => Database.Embedded.QueryEmbedd(_descriptionID);
        public string EmbeddedKey => _descriptionID;
        public static bool IsWellFood(BaseFoodSO food) => food is WellFoodSO;
        public static bool IsMaterial(BaseFoodSO food) => food is MaterialSO;
        public void Destroy()
        {
            Destroy(this);
        }
        public virtual void Set(BaseFoodSO baseFoodSO)
        {
            _nameID = baseFoodSO._nameID;
            _descriptionID = baseFoodSO._descriptionID;
            _icon = baseFoodSO._icon;
            _iconScale = baseFoodSO._iconScale;
            _price = baseFoodSO._price;
            _level = baseFoodSO._level;
            _calorie = baseFoodSO._calorie;
            _timeToPrepare = baseFoodSO._timeToPrepare;
            _isUnlocked = baseFoodSO._isUnlocked;
            NeedToWash = baseFoodSO.NeedToWash;
        }
        public bool Equals(BaseFoodSO other)
        {
            if (other == null) return false;
            return _nameID.Equals(other._nameID) && Level.Equals(other.Level);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + _nameID.GetHashCode() + Level.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            BaseFoodSO objAsPart = obj as BaseFoodSO;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            BaseFoodSO otherFood = obj as BaseFoodSO;
            if (otherFood != null)
                return NameID.CompareTo(otherFood.Level);
            return 0;
        }

    }
    public enum MaterialType
    {
        Vegetable,
        Meat,
        Starc,
        Spicy
    }
}

