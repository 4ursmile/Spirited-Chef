using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace UI
{
    public class UIFollow : MonoBehaviour, IUIFollowable
    {
        private RectTransform _rectTransform;
        [SerializeField] private bool _performingAnimation;
        private Image _image;
        public Image Image => _image;
        private Vector3 _followOffset;
        private Transform _target;
        [Header("Animation")]
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private Ease _animationEase = Ease.OutBack;
        [SerializeField] private Vector3 _animationMoveOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] private Vector3 _animationScaleOffset = new Vector3(0.5f, 0.5f, 0);
        private Vector3 _startPosition;
        private Vector3 _startScale;
        private void Awake() {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            gameObject.SetActive(false);
        }
        // Update is called once per frame
        void Update()
        {
            transform.position = _target.position + _followOffset;   
        }
        public void SetTarget(Transform target, Vector3 offset)
        {
            _startPosition = transform.localPosition;
            _startScale = transform.localScale;
            _followOffset = offset;
            _target = target;
            gameObject.SetActive(true);
            if (_performingAnimation)
            {
                transform.DOLocalMove(_animationMoveOffset, _animationDuration).SetEase(_animationEase).SetLoops(-1, LoopType.Yoyo);
                transform.DOScale(_animationScaleOffset.ElementWiseMultiply(transform.localScale), _animationDuration).SetEase(_animationEase).SetLoops(-1, LoopType.Yoyo);
            }
        }
        public void UnSetTarget()
        {
            transform.localPosition = _startPosition;
            transform.localScale = _startScale;   
            transform.DOPause();
            _target = null;
            gameObject.SetActive(false);
        }
    }
    public interface IUIFollowable
    {
        void SetTarget(Transform target, Vector3 offset);
        void UnSetTarget();
    }
}


