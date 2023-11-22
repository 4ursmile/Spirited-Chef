using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
namespace Manager
{
    [CreateAssetMenu(fileName = "InputUIBridgeSO", menuName = "ScriptableObjects/Manager/InputUIBridgeSO")]
    public class InputUIBridgeSO : ScriptableObject
    {
        [SerializeField] float _touchInteractiveRadius = 0.5f;
        [SerializeField] float _touchInteractiveTime = 0.5f;
        [SerializeField] float _touchInteractiveAlpha = 0.5f;
        [SerializeField] Ease _touchInteractiveEase = Ease.OutBack;
        
        private Image _touchInteractiveImage;
        public void TouchInScreen(Vector2 pos)
        {
            _touchInteractiveImage.gameObject.SetActive(true);
            _touchInteractiveImage.transform.DOPause();
            _touchInteractiveImage.DOPause();
            _touchInteractiveImage.transform.localScale = Vector3.zero;
            _touchInteractiveImage.transform.position = pos;
            _touchInteractiveImage.SetAlpha(_touchInteractiveAlpha);
            _touchInteractiveImage.transform.DOScale(Vector3.one*_touchInteractiveRadius, _touchInteractiveTime).SetUpdate(true).SetEase(_touchInteractiveEase);
            _touchInteractiveImage.DOFade(0, _touchInteractiveTime).SetUpdate(true).SetEase(_touchInteractiveEase).OnComplete(() => _touchInteractiveImage.gameObject.SetActive(false));
        }
        public void SetInteractiveImage(Image img)
        {
            _touchInteractiveImage = img;
        }

    }
}

