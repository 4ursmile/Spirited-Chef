using System;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CounterUI : MonoBehaviour
    {
        [SerializeField] Ease _easeType;
        [SerializeField] float _animationDuration = .3f;
        [SerializeField] float _scale = 1;
        [SerializeField] Vector3 _offset;
        [SerializeField] Ease _counterEaseType; 
        Image _counterImage;
        
        private void Awake() {
            _counterImage = GetComponent<Image>();
        }
        public void SetCounter(float duration, Vector3 position, bool flip = false, Action callback = null)
        {
            float startAmount = flip?1:0;
            float endAmount = 1-startAmount;
            transform.position = position;
            _counterImage.fillAmount = startAmount;
            var seq = BuildSequence(duration, position);
            seq.Insert(0, _counterImage.DOFillAmount(endAmount, duration).SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false)));
            seq.onComplete += () => callback?.Invoke();
            seq.Play();
        }
        private Sequence BuildSequence(float duration, Vector3 position)
        {
            Sequence sequence = DOTween.Sequence();
            transform.position = position + _offset/2;
            transform.localScale = Vector3.zero;
            sequence.Append(transform.DOScale(_scale, _animationDuration).SetEase(_easeType));
            sequence.Insert(0, transform.DOMove(position + _offset, duration).SetEase(_easeType));
            return sequence;
        }
    }
}
