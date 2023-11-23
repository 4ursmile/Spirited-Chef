using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
namespace Character
{
    public class CustomerCharacter : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] Ease _ease = Ease.Linear;
        [SerializeField] PathType _pathType = PathType.CatmullRom;
        [SerializeField] float _maximumTime = 10f;
        [SerializeField] float _maximumDistance = 10f;
        [SerializeField] InGameUIBridgeSO _inGameUIBridgeSO;
        private int _lastIndex;
        public int LastIndex => _lastIndex;
        private bool _isWalking = false;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        public void GoTo(Vector3[] path, int lastIndex = 0, Action onCompleteCallbacks = null)
        {
            if (path.Length == 0)
            {
                onCompleteCallbacks?.Invoke();
                return;
            }
            _animator.SetBool("IsWalking", true);
            _lastIndex = lastIndex;
            float timeScale = transform.position.GetDistanceScale(path[path.Length-1], _maximumDistance)*_maximumTime;

            transform.DOPath(path, timeScale, _pathType, PathMode.TopDown2D).SetEase(_ease).OnComplete(() => {
                _animator.SetBool("IsWalking", false);
                onCompleteCallbacks?.Invoke();

            });
        }
        public void GetOrder(float score)
        {
            _animator.SetTrigger("GetOrder");
            _inGameUIBridgeSO.OpenEmoji(UniversalObjectInstance.Instance.GetReactionKey(score), transform.position);
        }
        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }

}
