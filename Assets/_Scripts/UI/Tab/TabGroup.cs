using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using ObjectS;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace UI
{
    public class TabGroup : BaseUIPanel
    {

        [Header("TabGroupSetting")]
        [SerializeField] private TextMeshProUGUI _tabGroupName;
        [SerializeField] private TabItem tabItemPrefab;
        [SerializeField] private TabView tabViewPrefab;
        [SerializeField] private InGameUIBridgeSO _inGameUIBridgeSO;
        
        [SerializeField] private RectTransform _tabItemContainerRect;
        [SerializeField] private RectTransform _tabViewContainerRect;
        [SerializeField] private BaseFoodSO _defaultFood;
        [SerializeField] private float _tabItemWidth = 100f;
        [SerializeField] private float _tabItemHeight = 100f;
        [SerializeField] private float _tabItemMargin = 10f;
        private List<TabItem> tabItems;
        private List<TabView> objectsToSwap;
        private InteractiveObject _interactiveObject;
        public void InitTab(ListTabConfigSO listTabConfigSO, InteractiveObject interactiveObject = null)
        {
            // remove all children of tabItemContainer and tabViewContainer
            foreach (Transform child in _tabItemContainerRect)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in _tabViewContainerRect)
            {
                Destroy(child.gameObject);
            }
            objectsToSwap?.Clear();
            tabItems?.Clear();

            _interactiveObject = interactiveObject;
            _tabGroupName.text = LocalizationManager.Localize(listTabConfigSO.TabGroupName);
            _tabGroupName.font = LocalizationManager.GetFont();
            _tabItemContainerRect.sizeDelta = new Vector2(listTabConfigSO.TabItemObjects.Count * (_tabItemWidth + _tabItemMargin), _tabItemHeight);
            foreach (var tabItemObject in listTabConfigSO.TabItemObjects)
            {
                TabItem tabItem = Instantiate(tabItemPrefab, _tabItemContainerRect);
                tabItem.Init(this, tabItemObject);
                AddItem(tabItem);
                TabView tabView = Instantiate(tabViewPrefab, _tabViewContainerRect);
                tabView.Init(this, tabItemObject.FoodSOs);
                AddView(tabView);
            }
            _closeButton.OnClick += (Data) =>
            {
                OnTabViewItemSelected(null);
            };
            EnableUIPanel();
        }
        public void OnTabViewItemSelected(BaseFoodSO foodSO)
        {
            if (foodSO != null)
            {
                _interactiveObject.OnFoodSelected(Instantiate(foodSO));
                DisableUIPanel();
            } else
            {
                _interactiveObject.OnFoodSelected(null);
            }
        }
        public override void Init(OverlayUIManager manager)
        {
            base.Init(manager);
            _inGameUIBridgeSO.SetTabGroup(this);
        }
        public override void EnableUIPanel()
        {
            base.EnableUIPanel();
            if (selectedTab == null)
            {
                Selected(tabItems[0]);
            }
        }
        public void AddItem(TabItem item)
        {
            if (tabItems == null)
            {
                tabItems = new List<TabItem>();
            }
            tabItems.Add(item);
        }
        public void AddView(TabView view)
        {
            if (objectsToSwap == null)
            {
                objectsToSwap = new List<TabView>();
            }
            objectsToSwap.Add(view);
        }
        TabItem selectedTab;
        public void Selected(TabItem tabItemSelected)
        {

            if (selectedTab != null)
            {
                if (selectedTab == tabItemSelected)
                {
                    return;
                }
                selectedTab.Deselect();
            }
            selectedTab = tabItemSelected;
            selectedTab.Select();
            int index = tabItems.IndexOf(tabItemSelected);
            foreach (var tabView in objectsToSwap)
            {
                tabView?.gameObject.SetActive(false);
            }
            objectsToSwap[index].gameObject.SetActive(true);
        }
    }
}

