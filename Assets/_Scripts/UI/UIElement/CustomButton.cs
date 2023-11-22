
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;  
namespace UI
{
    [RequireComponent(typeof(Image))]
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _duration = 0.1f;
        [SerializeField] private Color _normalColor = new Color(1,1,1,1);
        [SerializeField] private Color _hoverColor = new Color(1,1,1,1);
        [SerializeField] private Color _pressedColor = new Color(1,1,1,1);
        private Image _image;
        [SerializeField] private UnityEvent _onClick;
        public UnityEvent onClick => _onClick;
        private void Awake() {
            _image = GetComponent<Image>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _image.DOColor(_pressedColor, _duration).onComplete += () => _image.DOColor(_normalColor, _duration);
            _onClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.DOColor(_hoverColor, _duration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.DOColor(_normalColor, _duration);
        }

    }

}
