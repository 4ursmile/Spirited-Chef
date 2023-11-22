using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Architecture;
using UnityEngine.UI;
namespace UI
{
    [RequireComponent(typeof(EventTrigger)), RequireComponent(typeof(Image))]
    public class ClickableBehaviour : MonoBehaviour
    {
        private Image _image;
        private EventTrigger _eventTrigger;
        [SerializeField] AudioClip _onClickClip;
        [SerializeField] SoundManagerSO _soundManager;
        [SerializeField] UnityEvent _onClick;
        public Action<BaseEventData> OnClick;
        public bool interactable 
        {
            get {
                return _eventTrigger.enabled;
            }
            set {
                if (_eventTrigger == null) return;
                _eventTrigger.enabled = value;
                _image.SetAlpha(value ? 1 : 0.5f);
            }
        }
        private void Awake()
        {
            _image = GetComponent<Image>();
            _eventTrigger = GetComponent<EventTrigger>();
            _eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown,
                callback = new EventTrigger.TriggerEvent()
            });
            _eventTrigger.triggers[0].callback.AddListener((data) => { _onClick?.Invoke(); OnClick?.Invoke(data); OnClickEventHandler();});
            Init();
        }
        protected virtual void Init()
        {

        }
        protected virtual void OnClickEventHandler(BaseEventData data = null)
        {
            if (_onClickClip == null) return;
            _soundManager.Play(_onClickClip);
        }

    }
}
