using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Architecture;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class OnUIPanelChange : MonoBehaviour
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [SerializeField] private AudioClip _appearSound;
        [SerializeField] private AudioClip _disappearSound;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private UnityEvent _onAppear;
        [SerializeField] private UnityEvent _onDisappear;
        [SerializeField] private SoundManagerSO _soundManager;
        

        public void EnableUIPanel()
        {
            gameObject.SetActive(true);
            RectTransform.localScale = Vector3.zero;
            if (_appearSound != null )
            {
                _soundManager.Play(_appearSound);
            }
            RectTransform.DOScale(Vector3.one, _duration).SetUpdate(true).SetEase(Ease.InCubic). OnComplete(() => {
                _onAppear?.Invoke();
            });
        }
        public void DisableUIPanel()
        {
            if (_disappearSound != null)
            {
                _soundManager.Play(_disappearSound);
            }
            RectTransform.localScale = Vector3.one;
            RectTransform.DOScale(Vector3.zero, _duration).SetUpdate(true).SetEase(Ease.OutCubic).OnComplete(() => {
                gameObject.SetActive(false);
                _onDisappear? .Invoke();
            });
        }
    }
}

