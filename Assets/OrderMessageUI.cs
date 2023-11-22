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
        _isOrderd = false;
    }
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Image _counterImage;   
    [SerializeField] private InGameUIBridgeSO _uiBridge;
    string _text;
    private bool _isOrderd = false;
    public bool IsOrderd => _isOrderd;
    private void OnEnable() {
        LocalizationManager.LocalizationChanged += OnLocalizationChanged;
    }
    public void SetText(string text, float waitingTime = 0)
    {
        _isOrderd = true;
        _text = text;
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBounce);
        _textMeshProUGUI.text = LocalizationManager.Localize(text);
        _textMeshProUGUI.font = LocalizationManager.GetFont();
        StartCoroutine(Counter(waitingTime));
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
    public IEnumerator Counter(float waitingTime)
    {
        _counterImage.fillAmount = 1;
        _counterImage.DOFillAmount(0, waitingTime).SetEase(Ease.OutCirc);
        yield return new WaitForSeconds(waitingTime);
        Close();
    }
    public void Close()
    {
        StopAllCoroutines();
        _isOrderd = false;
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).onComplete += () => gameObject.SetActive(false);
    }
}
