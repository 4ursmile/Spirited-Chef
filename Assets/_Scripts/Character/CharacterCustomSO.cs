using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Character
{
    [CreateAssetMenu(fileName = "CharacterCustom", menuName = "ScriptableObjects/CharacterCustom")]
    public class CharacterCustomSO : ScriptableObject
    {
        [SerializeField] string _hatStringAddress;
        [SerializeField] Color _hairColor = Color.black;
        [SerializeField] Color _skinColor = Color.yellow;
        [SerializeField] Color _shirtColor = Color.white;
        [SerializeField] string _name;
        [SerializeField] string _descriptionKey;
        [SerializeField] Color _normalOutlineColor = Color.black;
        [SerializeField] Color _selectedOutlineColor = Color.green;
        [SerializeField] float _outlineWidthNormal = 0.1f;
        [SerializeField] float _outlineWidthSelected = 0.2f;
        public string HatStringAddress { get => _hatStringAddress; }
        public Color HairColor { get => _hairColor; }
        public Color SkinColor { get => _skinColor; }
        public Color ShirtColor { get => _shirtColor; }
        public string Name { get => _name; }
        public string Description { get => _descriptionKey; }
        public Color NormalOutlineColor { get => _normalOutlineColor; }
        public Color SelectedOutlineColor { get => _selectedOutlineColor; }
        public float OutlineWidthNormal { get => _outlineWidthNormal; }
        public float OutlineWidthSelected { get => _outlineWidthSelected; }

    }
}

