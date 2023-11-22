using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;


namespace UI
{
    [CreateAssetMenu(fileName = "CardManagerSO", menuName = "ScriptableObjects/CardManagerSO", order = 1)]
    public class CardConfigSO : ScriptableObject
    {
        [field: SerializeField] public Sprite CardBack { get; private set; }
        [field: SerializeField] public SerializedDictionary<CardTier, float> WaitingCardOpenTimes { get; private set; }
        [field: SerializeField] public SerializedDictionary<CardTier, float> CardDelayTimes { get; private set; }
        [field: SerializeField] public SerializedDictionary<CardTier, float> CardOpenTimes { get; private set; }
        [field: SerializeField] public SerializedDictionary<CardTier, float> CardSeletedShowTimes { get; private set; }
        [field: Header("Card Select Sound")]
        [field: SerializeField] public SerializedDictionary<CardTier, AudioClip> CardSelectSounds { get; private set; }
        [field: SerializeField] public SerializedDictionary<CardTier, AudioClip> CardDisplaySounds { get; private set; }
        [field: Header("Card Open/Close Sound")]
        [field: SerializeField] public SerializedDictionary<CardTier, AudioClip> CardOpenSounds { get; private set; }
        [field: SerializeField] public SerializedDictionary<CardTier, AudioClip> CardCloseSounds { get; private set; }


        [Header("Frame")]
        [SerializeField] SerializedDictionary<CardTier, Sprite> Borders;
        [Header("Gem")]
        [SerializeField] SerializedDictionary<CardTier, Sprite> Gems;
        [Header("Color Back")]
        [SerializeField] SerializedDictionary<CardTier, Color> ColorBacks;
        [Header("Color Door Top")]
        [SerializeField] SerializedDictionary<CardTier, Color> ColorDoorTops;
        [Header("Color Door Bottom Left")]
        [SerializeField] SerializedDictionary<CardTier, Color> ColorDoorBottomLefts;
        [Header("Color Door Bottom Right")]
        [SerializeField] SerializedDictionary<CardTier, Color> ColorDoorBottomRights;
        [Header("Color Badge")]
        [SerializeField] SerializedDictionary<CardTier, Color> ColorBadges;
        [Header("Color Panel")]
        [SerializeField] SerializedDictionary<CardTier, Color> ColorPanels;
        public CardSetup GetCardSetup(CardTier tier)
        {
            return new CardSetup()
            {
                Border = Borders[tier],
                Gem = Gems[tier],
                ColorBack = ColorBacks[tier],
                ColorDoorTop = ColorDoorTops[tier],
                ColorDoorBottomLeft = ColorDoorBottomLefts[tier],
                ColorDoorBottomRight = ColorDoorBottomRights[tier],
                ColorBadge = ColorBadges[tier],
                CardOpenTime = CardOpenTimes[tier],
                CardOpenSound = CardOpenSounds[tier],
                CardCloseSound = CardCloseSounds[tier]
            };
        }
        public Color GetColorPanel(CardTier tier)
        {
            return ColorPanels[tier];
        }
        public Sprite GetGem(CardTier tier)
        {
            return Gems[tier];
        }
        public float GetCardOpenTime(CardTier tier)
        {
            return CardOpenTimes[tier];
        }
    }
    public class CardSetup
    {
        public Sprite Border { get; set; }
        public Sprite Gem { get; set; }
        public Color ColorBack { get; set; }
        public Color ColorDoorTop { get;  set; }
        public Color ColorDoorBottomLeft { get; set; }
        public Color ColorDoorBottomRight { get; set; }
        public Color ColorBadge { get; set; }
        public float CardOpenTime { get; set; }
        public AudioClip CardOpenSound { get; set;}
        public AudioClip CardCloseSound { get; set; }
    }
    public enum CardTier
    {
        Common,
        Epic,
        Legendary
    }
}

