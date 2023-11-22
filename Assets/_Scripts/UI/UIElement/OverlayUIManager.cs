using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architecture;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
namespace UI
{
    public class OverlayUIManager : MonoBehaviour
    {
        [SerializeField] List<BaseUIPanel> _uiPanels;
        [SerializeField] private SoundManagerSO _soundManager;
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Image _progressBar;
        [SerializeField] [Range(0,1)] private float _tagerAlpha = 0.5f;
        [SerializeField] private float _duration = 0.3f;
        private int _currentPanelCount = 0;
        private void Awake()
        {
            _background.gameObject.SetActive(false);
            foreach (var uiPanel in _uiPanels)
            {
                uiPanel.Init(this);
            }
            _currentPanelCount = 0;
        }
        private void Start() {
            UniversalObjectInstance.Instance.OnChangeMoney += SetMoneyUI; 
            UniversalObjectInstance.Instance.ChangeMoney(0, Vector3.zero);
        }
        private void OnDestroy() {
            UniversalObjectInstance.Instance.OnChangeMoney -= SetMoneyUI;    
        }
        public void SetMoneyUI(int currentMoney, int maxMoney)
        {
            _moneyText.text = $"{currentMoney}/{maxMoney}";
            _progressBar.fillAmount = (float)currentMoney / maxMoney;
        }
        public void PanelOpen()
        {
            _currentPanelCount++;
            if (_currentPanelCount == 1)
            {
                ShowBackground();
            }
        }
        public void PanelClose()
        {
            _currentPanelCount--;
            if (_currentPanelCount <= 0)
            {
                _currentPanelCount = 0;
                _background.gameObject.SetActive(false);
                HideBackground();
            }
        }
        private void ShowBackground()
        {
            _background.gameObject.SetActive(true);
            _background.SetAlpha(0);
            _background.DOFade(_tagerAlpha, _duration).SetUpdate(true);
        }
        private void HideBackground()
        {
            _background.DOFade(0, _duration).SetUpdate(true).OnComplete(() => {
                _background.gameObject.SetActive(false);
            });
        }

    }
}

