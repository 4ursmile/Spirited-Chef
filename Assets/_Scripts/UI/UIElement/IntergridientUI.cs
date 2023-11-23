
using System;
using System.Collections.Generic;
using DG.Tweening;
using ObjectS;
using UnityEngine;

namespace UI 
{
    public class IntergridientUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _selfRect;
        [SerializeField] private ImageItem _imageItem;
        [SerializeField] private float _height;
        [SerializeField] private float _width;
        [SerializeField] private float _space;
        [SerializeField] private float _deltaPaddingHoz = .6f;
        [Header("animation")]
        [SerializeField] private float _timeOpenUp;
        [SerializeField] private float _timeOpenDown;
        
        [SerializeField] private Ease _easeOpenUp;
        [SerializeField] private Vector3 _offset;
        private List<ImageItem> _listImageItem = new List<ImageItem>();
        private Action _closeCallback;
        public void SetRecipe(List<BaseFoodSO> baseFoodSOs, Vector3 position, Action closeCallback = null)
        {
            _listImageItem.Clear();
            _selfRect.sizeDelta = new Vector2(_width * baseFoodSOs.Count + _space * (baseFoodSOs.Count - 1) + _deltaPaddingHoz, _height);
            for (int i = 0; i < baseFoodSOs.Count; i++)
            {
                var imageItem = Instantiate(_imageItem, _rectTransform.transform);
                imageItem.SetImage(baseFoodSOs[i].Icon);
                _listImageItem.Add(imageItem);
            }
            _closeCallback = closeCallback;
            OpenUp(position);
        }
        public void SetRecipe(BaseFoodSO baseFoodSO, Vector3 position, Action closeCallback = null)
        {
            _listImageItem.Clear();
            _selfRect.sizeDelta = new Vector2(_width, _height);
            var imageItem = Instantiate(_imageItem, _rectTransform.transform);
            imageItem.SetImage(baseFoodSO.Icon);
            _listImageItem.Add(imageItem);
            _closeCallback = closeCallback;
            OpenUp(position);
        }
        public void RemoveItemAt(int index)
        {
            Destroy(_listImageItem[index].gameObject);
            _listImageItem.RemoveAt(index);

            if (_listImageItem.Count == 0)
            {
                Close();
                return;
            }
            _selfRect.sizeDelta = new Vector2(_width * _listImageItem.Count + _space * (_listImageItem.Count - 1) + _deltaPaddingHoz, _height);

        }
        public void OpenUp(Vector3 position)
        {
            transform.position = position;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _timeOpenUp).SetUpdate(true).SetEase(_easeOpenUp);
            transform.DOMove(position + _offset, _timeOpenUp).SetUpdate(true).SetEase(_easeOpenUp);
        }
        public void Close()
        {
            transform.DOScale(Vector3.zero, _timeOpenDown).SetUpdate(true).SetEase(_easeOpenUp);
            transform.DOMove(transform.position - _offset, _timeOpenDown).SetUpdate(true).SetEase(_easeOpenUp).OnComplete(() => {
                _closeCallback?.Invoke();
            });
        }
    }
}

