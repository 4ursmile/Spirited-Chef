using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using DG.Tweening;
using UnityEngine.UI;   
using TMPro;
using UnityEngine;
using Architecture;

public class OrderMessageUI : MonoBehaviour
{
    private void Awake() {
        _uiBridge.SetOrderMessageUI(this);
    }
    private void Start() {
        Close();
    }
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Image _counterImage;   
    [SerializeField] private InGameUIBridgeSO _uiBridge;
    [SerializeField] private Ease ease;
    string _text;
    private void OnEnable() {
        LocalizationManager.LocalizationChanged += OnLocalizationChanged;
    }
    public void SetText(string text, float waitingTime = 0)
    {

        _text = text;
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .5f).SetUpdate(true).SetEase(Ease.OutBounce);
        _textMeshProUGUI.text = LocalizationManager.Localize(text);
        _textMeshProUGUI.font = LocalizationManager.GetFont();
        _counterImage.fillAmount = 1;
        _counterImage.DOFillAmount(0, waitingTime).SetUpdate(true).SetEase(ease).onComplete += () => {
            Close();
        };
    }
    private void OnDisable() {
        LocalizationManager.LocalizationChanged -= OnLocalizationChanged;
    }
    private void OnLocalizationChanged()
    {
        if (_text == null)
            return;
        _textMeshProUGUI.text = LocalizationManager.Localize(_text);
        _textMeshProUGUI.font = LocalizationManager.GetFont();
    }
    public void Close()
    {
        transform.DOScale(Vector3.zero, .3f).SetUpdate(true).SetEase(Ease.InBack).onComplete += () => gameObject.SetActive(false);
        _counterImage.DOKill();
    }
}
