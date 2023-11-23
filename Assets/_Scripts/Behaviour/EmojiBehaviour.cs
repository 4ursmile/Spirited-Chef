using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Architecture;
namespace Behaviour
{
    public class EmojiBehaviour : MonoBehaviour
    {
        [SerializeField] Vector3 offset;
        [SerializeField] float _duration = 1f;
        [SerializeField] Ease _ease = Ease.OutBounce;
        [SerializeField] Vector3 _scale = Vector3.one;
        [SerializeField] Vector3 _animationOffset = Vector3.up;
        [SerializeField] SoundManagerSO _soundManagerSO;
        [SerializeField] AudioClip _popClip;

        private SpriteRenderer _spriteRenderer;
        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void SetSpriteByKey(string spriteKey, Vector3 position, Action OnCompleteAction = null)
        {
            transform.position = position + offset;
            _spriteRenderer.sprite = Database.GetEmoji(spriteKey);
            DoAnimation(OnCompleteAction);
            _soundManagerSO.Play(_popClip);
        }
        public void SetSprite(Sprite sprite, Vector3 position, Action OnCompleteAction = null)
        {
            transform.position = position + offset;
            _spriteRenderer.sprite = sprite;
            DoAnimation(OnCompleteAction);
            _soundManagerSO.Play(_popClip);

        }
        private void DoAnimation(Action OnCompleteAction = null)
        {
            transform.localScale = Vector3.one*0.2f;
            _spriteRenderer.SetAlpha(.1f);
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(_scale, _duration/1.2f).SetEase(_ease));
            sequence.Join(_spriteRenderer.DOFade(1, _duration/1.2f).SetEase(_ease));
            sequence.Join(transform.DOMove(transform.position + _animationOffset, _duration).SetEase(_ease));
            sequence.Append(transform.DOScale(Vector3.zero, _duration/3).SetEase(_ease));
            sequence.Join(_spriteRenderer.DOFade(0, _duration/3).SetEase(_ease));
            sequence.Join(transform.DOMove(transform.position + _animationOffset*0.1f, _duration/3).SetEase(_ease));

            sequence.onComplete += () => {
                OnCompleteAction?.Invoke();
            };
        }
    }
}

