using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using ObjectS;
using Unity.VisualScripting;
using UnityEngine;
namespace UI
{
    public class TabView : MonoBehaviour
    {
        private TabGroup _tabGroup;
        [SerializeField] private RectTransform _contentTransform;
        [SerializeField] private TabViewItem _tabViewItemPrefab;
        public void Init(TabGroup tabGroup, List<BaseFoodSO> foods)
        {
            _tabGroup = tabGroup;
            foreach (var item in foods)
            {
                AddTabItem(item);
            }
        }
        public void AddTabItem(BaseFoodSO baseFoodSO)
        {
            if (baseFoodSO == null)
            {
                return;
            }
            TabViewItem tabItem = Instantiate(_tabViewItemPrefab, _contentTransform);
            tabItem.Init(this, baseFoodSO);
        }
        public virtual void OnTabItemSelected(TabViewItem tabItem)
        {
            _tabGroup.OnTabViewItemSelected(tabItem.CurrentFood);
        }
        
    }

}
