using System.Collections;
using System.Collections.Generic;
using Architecture;
using Assets.SimpleLocalization.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InforUI : MonoBehaviour, IUIFollowable
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private InGameUIBridgeSO _bridgeSO;
        [SerializeField] private SoundManagerSO _soundManagerSO;
        [SerializeField] private AudioClip _appearSound;
        [SerializeField] private float _animationDuration = .3f;
        [SerializeField] private Ease _easeType;
        public void SetTarget(Transform target, Vector3 offset)
        {
            transform.DOKill();
            gameObject.SetActive(true);
            transform.position = target.position + offset;
            _soundManagerSO.Play(_appearSound);
            transform.localScale = Vector3.zero;
            transform.DOScale(_bridgeSO.InforUIScale, _animationDuration).SetEase(_easeType);
        }

        public void UnSetTarget()
        {
            _soundManagerSO.Play(_appearSound);
            transform.DOKill();
            transform.DOScale(Vector3.zero, _animationDuration).SetEase(_easeType).OnComplete(() => gameObject.SetActive(false));
        }
        public void SetInfor(string name, string description)
        {
            _nameText.text = LocalizationManager.Localize(name);
            _descriptionText.text = LocalizationManager.Localize(description);
        }

        // Start is called before the first frame update
        void Start()
        {
            _bridgeSO.SetInforUI(this);
            LocalizationManager.LocalizationChanged += OnLocalChanged;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        private void OnLocalChanged()
        {
            _nameText.font = LocalizationManager.GetFont();
            _descriptionText.font = LocalizationManager.GetFont();
        }
        private void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= OnLocalChanged;
        }
    }
}

