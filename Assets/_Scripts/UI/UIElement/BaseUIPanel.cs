using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Architecture;
using DG.Tweening;
using UnityEngine.UI;
namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class BaseUIPanel : MonoBehaviour
    {
    
        public RectTransform RectTransform { get; private set; }
        [Header("UI Panel")]
        [SerializeField] protected AudioClip _appearSound;
        [SerializeField] protected AudioClip _disappearSound;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private Ease _ease = Ease.InCubic;
        [SerializeField] protected UnityEvent _onAppear;
        [SerializeField] protected UnityEvent _onDisappear;
        [SerializeField] protected SoundManagerSO _soundManager;
        [SerializeField] protected ClickableBehaviour _closeButton;
        private OverlayUIManager _manager;
        public bool IsOpen => gameObject.activeSelf;
        public virtual void Init(OverlayUIManager manager)
        {
            _manager = manager;
            RectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
            if (_closeButton != null)
            {
                 _closeButton.OnClick += (data) => { DisableUIPanel(); };

            }
        }
        public virtual void EnableUIPanel()
        {
            if (IsOpen) return;
            _manager.PanelOpen();
            gameObject.SetActive(true);
            RectTransform.localScale = Vector3.zero;
            if (_appearSound != null )
            {
                _soundManager.Play(_appearSound);
            }
            RectTransform.DOScale(Vector3.one, _duration).SetUpdate(true).SetEase(_ease). OnComplete(() => {
                _onAppear?.Invoke();
            });
        }
        public virtual void DisableUIPanel()
        {
            if (!IsOpen) return;
            _manager.PanelClose();
            if (_disappearSound != null)
            {
                _soundManager.Play(_disappearSound);
            }
            RectTransform.localScale = Vector3.one;
            RectTransform.DOScale(Vector3.zero, _duration).SetUpdate(true).SetEase(_ease).OnComplete(() => {
                gameObject.SetActive(false);
                _onDisappear? .Invoke();
            });
        }
    }
}
