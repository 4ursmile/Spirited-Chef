
using System.Collections.Generic;
using UnityEngine;
using Architecture;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;
namespace UI
{
    public class CardManager : BaseUIPanel
    {
        [SerializeField] CardConfigSO _cardConfigSO;
        [SerializeField] private List<CardHandle> _cardHandles;
        [SerializeField] private GameObject _panelCard;
        [SerializeField] private Image _rollButton;
        private ClickableBehaviour _rollButtonBtn;
        [SerializeField] private Image _rollButtonGem;
        [SerializeField] private TextMeshProUGUI _rollButtonText;
        [SerializeField] private float _cardAppearTimeDelay = 0.1f;
        [SerializeField] [Range(0,1)] private float _panelAlpha = 0.8f;
        [SerializeField] float _animationTime = 0.5f;
        [SerializeField] float _cardRollDeltaTime = 0.3f;
        [SerializeField] CardManagerSO _cardManagerSO;
        [SerializeField] InGameUIBridgeSO _inGameUIBridgeSO;
        [SerializeField] Ease _easeType;
        private bool _isCardOpen = false;
        private CardTier _currentTier = CardTier.Common;
        private CardSetup _currentCardSetup;
        public override void Init(OverlayUIManager manager)
        {
            base.Init(manager);
            _cardManagerSO.Init(this);
            _inGameUIBridgeSO.SetCardManager(this);
            foreach (var cardHandle in _cardHandles)
            {
                cardHandle.Initialize(this);
            }
            _rollButtonBtn = _rollButton.GetComponent<ClickableBehaviour>();
            _rollButtonBtn.interactable = false;
            _rollButtonText.text = _cardManagerSO.CurrentCardRollNumber.ToString();
        }
        public override void EnableUIPanel()
        {
            base.EnableUIPanel();
            _currentTier = CardTier.Epic;
            OpenCardPanel(_currentTier);
        }

        public void OpenCardPanel(CardTier tier)
        {
            GameManager.Instance.PauseGame();
            var setup = _cardConfigSO.GetCardSetup(tier);
            _currentCardSetup = setup;
            _rollButton.color = setup.ColorBack;
            _rollButtonGem.sprite = setup.Gem;
            _rollButtonBtn.interactable = false;
            List<CardInforSO> cardInforSOs = _cardManagerSO.CardPool.GetCardInforSOsRandom(tier, _cardHandles.Count);
            foreach (var (cardHandle, index) in _cardHandles.Enumerate())
            {
                cardHandle.Init(setup);
                cardHandle.SetCard(cardInforSOs[index]);
            }
            _panelCard.SetActive(true);
            _soundManager.Play(_cardConfigSO.CardDisplaySounds[tier]);
            _panelCard.transform.localScale = Vector3.zero;
            _panelCard.transform.DOScale(Vector3.one, _animationTime).SetEase(_easeType).SetUpdate(true).onComplete += () =>
            {
                _isCardOpen = true;

                OpenAllCard(()=>
                {
                    if (_cardManagerSO.CurrentCardRollNumber > 0)
                    {
                        _rollButtonBtn.interactable = true;
                        _rollButtonText.text = _cardManagerSO.CurrentCardRollNumber.ToString();
                    } else
                    {
                        _rollButtonBtn.interactable = false;
                        _rollButtonText.text = "";
                    }
                });
            };

        }
        public void OpenAllCard(Action onComplete = null)
        {
            var sequence = DOTween.Sequence();
            sequence.SetUpdate(true);
            sequence.AppendInterval(_cardConfigSO.WaitingCardOpenTimes[_currentTier]).SetUpdate(true);
            foreach (var cardHandle in _cardHandles)
            {
                sequence.AppendCallback(() => cardHandle.CardDoorOpen());
                sequence.AppendInterval(_cardConfigSO.CardDelayTimes[_currentTier]).SetUpdate(true);
            }
            sequence.Play().OnComplete(() => {
                onComplete?.Invoke();
            });
        }
        
        public void CloseCardPanel()
        {
            DisableUIPanel();
        }

        private void CloseAllCard(bool PerformAnimation = true)
        {
            _rollButtonBtn.interactable = false;
            foreach (var cardHandle in _cardHandles)
            {
                cardHandle.CardDoorClose(PerformAnimation);
            }
        }
        public bool IsAllCardOpen =>  _cardHandles.TrueForAll(x => x.IsOpen);
        public void RollCards()
        {
            _cardManagerSO.Add2RollNumber(-1);
            _rollButton.transform.DOPunchScale(Vector3.one * 0.2f, _animationTime).SetEase(_easeType).SetUpdate(true);
            _rollButtonGem.transform.DOPunchScale(Vector3.one * 0.2f, _animationTime).SetEase(_easeType).SetUpdate(true);
            var sequence = DOTween.Sequence();
            sequence.SetUpdate(true);
            sequence.AppendCallback(() => CloseAllCard());
            sequence.AppendInterval(_cardConfigSO.CardOpenTimes[_currentTier]+_cardRollDeltaTime).SetUpdate(true);
            sequence.Play().OnComplete(() => {
                ChangeCard();
                OpenAllCard(()=>
                {
                    if (_cardManagerSO.CurrentCardRollNumber > 0)
                    {
                        _rollButtonBtn.interactable = true;
                        _rollButtonText.text = _cardManagerSO.CurrentCardRollNumber.ToString();
                    } else
                    {
                        _rollButtonBtn.interactable = false;
                        _rollButtonText.text = "";
                    }
                });
                
            });
        }
        public void ChangeCard()
        {
            var tier = _currentTier;
            var cards = _cardManagerSO.GetCards(tier, _cardHandles.Count);
            foreach (var (cardHandle, index) in _cardHandles.Enumerate())
            {
                cardHandle.SetCard(cards[index]);
            }
        }
        public void CardSelected(CardHandle cardHandle)
        {
            GameManager.Instance.ResumeGame();
            _soundManager.Play(_cardConfigSO.CardSelectSounds[_currentTier]);
            _rollButtonBtn.interactable = false;
            var sequence = DOTween.Sequence();
            sequence.SetUpdate(true);
            sequence.AppendInterval(_currentCardSetup.CardOpenTime).SetUpdate(true);
            foreach(var card in _cardHandles)
            {
                if (card != cardHandle)
                {
                    card.CardDoorClose(true, () => {
                        card.CardDisappear();
                        }   
                    );
                }
            }
            float showTime = _cardConfigSO.CardSeletedShowTimes[_currentTier];
            cardHandle.CardSelectedShow(showTime, () => {
                CloseCardPanel();
                cardHandle.CardInfor.ApplyEffect(_cardManagerSO);
            });
            sequence.Play();
        }

    }

}
