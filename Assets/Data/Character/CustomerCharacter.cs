using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
namespace Character
{
    public class CustomerCharacter : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] Ease _ease = Ease.Linear;
        [SerializeField] PathType _pathType = PathType.CatmullRom;
        [SerializeField] float _maximumTime = 10f;
        [SerializeField] float _maximumDistance = 10f;
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }
        public void GoTo(Vector3[] path, Action onCompleteCallbacks = null)
        {
            _animator.SetBool("IsWalking", true);
            float timeScale = transform.position.GetDistanceScale(path[path.Length-1], _maximumDistance)*_maximumTime;
            transform.DOPath(path, timeScale, _pathType).SetEase(_ease).OnComplete(() => {
                _animator.SetBool("IsWalking", false);
                onCompleteCallbacks?.Invoke();
            });
        }
        public void GetOrder()
        {
            _animator.SetTrigger("GetOrder");
        }
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }

}
