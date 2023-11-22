using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using ObjectS;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace UI 
{
    public class TabViewItem : ClickableBehaviour
    {
        private TabView _tabView;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private Image _iconImage;
        private BaseFoodSO _currentFood;
        public BaseFoodSO CurrentFood => _currentFood;
        public void Init(TabView tabView, BaseFoodSO food)
        {
            _tabView = tabView;
            _currentFood = food;
            _iconImage.sprite = food.Icon;
            _nameText.text = LocalizationManager.Localize(food.NameID);
            _nameText.font = LocalizationManager.GetFont();
            _coinText.text = food.Price.ToString();
        }

        protected override void OnClickEventHandler(BaseEventData data = null)
        {
            _tabView.OnTabItemSelected(this);
        }
    }
}

