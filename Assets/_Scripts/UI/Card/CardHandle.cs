using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using DG.Tweening;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHandle : ClickableBehaviour
{
    [SerializeField] private Image _baseCardImage;
    [SerializeField] private TextMeshProUGUI _cardNameText;
    [SerializeField] private TextMeshProUGUI _cardDescriptionText;
    [SerializeField] private Image _cardImage;
    [SerializeField] private Image _cardFrameImage;
    [SerializeField] private Image _cardDoorTop;
    [SerializeField] private Image _cardDoorBottomLeft;
    [SerializeField] private Image _cardDoorBottomRight;
    [SerializeField] private Image _cardBadge;
    [SerializeField] private Ease _easeDoorType;
    [SerializeField] private float _cardAppearTime = 0.5f;
    [SerializeField] private SoundManagerSO _soundManagerSO;
    [SerializeField] private AudioClip _cardOpenSound;
    [SerializeField] private AudioClip _cardCloseSound;
    private CardManager _cardManager;
    private bool _isOpen = false;
    public bool IsOpen => _isOpen;
    private Vector2 _startWidthHeight;
    private RectTransform _rectTransform;
    public RectTransform rectTransform => _rectTransform;
    public CardInforSO CardInfor { get; private set; }
    public void SetCard(CardInforSO cardInforSO)
    {
        _cardNameText.text = cardInforSO.CardName;
        _cardDescriptionText.text = cardInforSO.CardDescription;
        _cardImage.sprite = cardInforSO.CardImage;
        CardInfor = cardInforSO;
    }
    public void Initialize(CardManager cardManager)
    {
        _cardManager = cardManager;
        _rectTransform = GetComponent<RectTransform>();
        _startWidthHeight = _rectTransform.sizeDelta;
    }
    public void CardDoorOpen(bool PerformAnimation = true)
    {
        if (_isOpen) return;
        if (!PerformAnimation)
        {
            _baseCardImage.raycastTarget = true;
            _isOpen = true;
            _cardDoorTop.gameObject.SetActive(false);
            _cardDoorBottomLeft.gameObject.SetActive(false);
            _cardDoorBottomRight.gameObject.SetActive(false);
            _cardDoorTop.rectTransform.anchoredPosition = new Vector2(0, _cardDoorTop.rectTransform.sizeDelta.y/2);
            _cardDoorBottomLeft.rectTransform.anchoredPosition = new Vector2(-_cardDoorBottomLeft.rectTransform.sizeDelta.x/2, 0);
            _cardDoorBottomRight.rectTransform.anchoredPosition = new Vector2(_cardDoorBottomRight.rectTransform.sizeDelta.x/2, 0);
            return;
        }
        _baseCardImage.raycastTarget = false;
        _cardDoorTop.gameObject.SetActive(true);
        _cardDoorBottomLeft.gameObject.SetActive(true);
        _cardDoorBottomRight.gameObject.SetActive(true);

        _cardDoorTop.rectTransform.anchoredPosition = new Vector2(0, -_cardDoorTop.rectTransform.sizeDelta.y/2);
        _cardDoorBottomLeft.rectTransform.anchoredPosition = new Vector2(_cardDoorBottomLeft.rectTransform.sizeDelta.x/2, 0);
        _cardDoorBottomRight.rectTransform.anchoredPosition = new Vector2(-_cardDoorBottomRight.rectTransform.sizeDelta.x/2, 0);
        
        var seq = DOTween.Sequence();
        seq.Append(_cardDoorTop.rectTransform.DOAnchorPosY(_cardDoorTop.rectTransform.sizeDelta.y/2, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType));
        seq.Insert(_cardAppearTime/2, _cardDoorBottomLeft.rectTransform.DOAnchorPosX(-_cardDoorBottomLeft.rectTransform.sizeDelta.x/2, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType));
        seq.Insert(_cardAppearTime/2,_cardDoorBottomRight.rectTransform.DOAnchorPosX(_cardDoorBottomRight.rectTransform.sizeDelta.x/2, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType));
        seq.Play().SetUpdate(true).onComplete += () =>
        {
            _baseCardImage.raycastTarget = true;
            _isOpen = true;
            _cardDoorTop.gameObject.SetActive(false);
            _cardDoorBottomLeft.gameObject.SetActive(false);
            _cardDoorBottomRight.gameObject.SetActive(false);
        };
        _soundManagerSO.Play(_cardOpenSound);
    }
    public void CardDoorClose(bool PerformAnimation = true, Action OnComplete = null)
    {
        if (!_isOpen) return;
        if (!PerformAnimation)
        {
            _baseCardImage.raycastTarget = false;
            _cardDoorTop.gameObject.SetActive(true);
            _cardDoorBottomLeft.gameObject.SetActive(true);
            _cardDoorBottomRight.gameObject.SetActive(true);
            _cardDoorTop.rectTransform.anchoredPosition = new Vector2(0, -_cardDoorTop.rectTransform.sizeDelta.y/2);
            _cardDoorBottomLeft.rectTransform.anchoredPosition = new Vector2(_cardDoorBottomLeft.rectTransform.sizeDelta.x/2, 0);
            _cardDoorBottomRight.rectTransform.anchoredPosition = new Vector2(-_cardDoorBottomRight.rectTransform.sizeDelta.x/2, 0);
            _isOpen = false;
            OnComplete?.Invoke();
            return;
        }
        _baseCardImage.raycastTarget = false;
        _cardDoorTop.gameObject.SetActive(true);
        _cardDoorBottomLeft.gameObject.SetActive(true);
        _cardDoorBottomRight.gameObject.SetActive(true);

        _cardDoorTop.rectTransform.anchoredPosition = new Vector2(0,_cardDoorTop.rectTransform.sizeDelta.y/2);
        _cardDoorBottomLeft.rectTransform.anchoredPosition = new Vector2(-_cardDoorBottomLeft.rectTransform.sizeDelta.x/2, 0);
        _cardDoorBottomRight.rectTransform.anchoredPosition = new Vector2(_cardDoorBottomRight.rectTransform.sizeDelta.x/2, 0);

        var seq = DOTween.Sequence();
        seq.Append(_cardDoorBottomLeft.rectTransform.DOAnchorPosX(_cardDoorBottomLeft.rectTransform.sizeDelta.x/2, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType));
        seq.Join(_cardDoorBottomRight.rectTransform.DOAnchorPosX(-_cardDoorBottomRight.rectTransform.sizeDelta.x/2, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType));
        seq.Insert(_cardAppearTime/2, _cardDoorTop.rectTransform.DOAnchorPosY(-_cardDoorTop.rectTransform.sizeDelta.y/2, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType));
        seq.Play().SetUpdate(true).onComplete += () =>
        {
            _isOpen = false;
            OnComplete?.Invoke();
        };
        _soundManagerSO.Play(_cardCloseSound);
    }
    public void CardDisappear()
    {
        _startWidthHeight = _rectTransform.sizeDelta;
        _rectTransform.DOSizeDelta(Vector2.zero, _cardAppearTime).SetUpdate(true).SetEase(_easeDoorType).onComplete += () =>
        {
            gameObject.SetActive(false);
        };

    }
    public void Init(CardSetup cardSetup)
    {
        _cardFrameImage.sprite = cardSetup.Border;
        _baseCardImage.color = cardSetup.ColorBack;
        _cardDoorTop.color = cardSetup.ColorDoorTop;
        _cardDoorBottomLeft.color = cardSetup.ColorDoorBottomLeft;
        _cardDoorBottomRight.color = cardSetup.ColorDoorBottomRight;
        _cardBadge.color = cardSetup.ColorBadge;
        _cardBadge.sprite = cardSetup.Gem;
        _cardAppearTime = cardSetup.CardOpenTime;
        _cardOpenSound = cardSetup.CardOpenSound;
        _cardCloseSound = cardSetup.CardCloseSound;
        _isOpen = true;
        CardDoorClose(false);
        gameObject.SetActive(true);
        _rectTransform.sizeDelta = _startWidthHeight;
        transform.localScale = Vector3.one;
        _cardImage.SetAlpha(1);
        _cardFrameImage.SetAlpha(1);
    }

    protected override void OnClickEventHandler(BaseEventData data = null)
    {
        base.OnClickEventHandler(data);
        _cardManager.CardSelected(this);
        _baseCardImage.raycastTarget = false;
    }
    public void CardSelectedShow(float timeShow, Action OnComplete = null)
    {
        _baseCardImage.raycastTarget = false;
        var seq = DOTween.Sequence();
        seq.SetUpdate(true);
        seq.Append(transform.DOScale(1.2f, timeShow).SetUpdate(true).SetEase(_easeDoorType));
        seq.Play().onComplete += () =>
        {
            OnComplete?.Invoke();
        };

    }
}
