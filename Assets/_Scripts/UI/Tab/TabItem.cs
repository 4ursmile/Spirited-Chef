using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
namespace UI
{

    public class TabItem : ClickableBehaviour
    {
        [SerializeField] private Image _tabIcon;
        [SerializeField] private Image _tabBackground;
        private TabGroup _tabGroup;
        [SerializeField] private TabConFigSO _tabConfig;
        [SerializeField] private float _transitionDuration = 0.3f;

        private void Start() {
            Deselect();
        }
        protected override void OnClickEventHandler(BaseEventData data = null)
        {
            _tabGroup.Selected(this);
        }
        public void Init(TabGroup tabGroup, TabItemObject tabItemObject)
        {
            _tabGroup = tabGroup;
            _tabIcon.sprite = tabItemObject.TabIcon;
        }
        public void Select()
        {
            _tabBackground.DOColor(_tabConfig.TabSelectedBackgroundColor, _transitionDuration);
            _tabBackground.rectTransform.DOScale(_tabConfig.TabSelectedBackgroundSize, _transitionDuration);
            _tabIcon.DOColor(_tabConfig.TabSelectedIconColor, _transitionDuration);
            _tabIcon.rectTransform.DOScale(_tabConfig.TabSelectedIconSize, _transitionDuration);
        }
        public void Deselect()
        {
            _tabBackground.DOColor(_tabConfig.TabIdleBackgroundColor, _transitionDuration);
            _tabBackground.rectTransform.DOScale(_tabConfig.TabIdleBackgroundSize, _transitionDuration);
            _tabIcon.DOColor(_tabConfig.TabIdleIconColor, _transitionDuration);
            _tabIcon.rectTransform.DOScale(_tabConfig.TabIdleIconSize, _transitionDuration);
        }

    }
}

