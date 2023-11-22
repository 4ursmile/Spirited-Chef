using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;
using UnityEngine.SocialPlatforms;
using UnityEngine.EventSystems;
namespace UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocalizationDropDownTMP : ClickableBehaviour
    {
        [field: SerializeField] public List<string> LocalizationKeys { get; set; }
        private TMP_Dropdown _dropdown;
        public TMP_Dropdown Dropdown {
            get => _dropdown;
            set => _dropdown = value;
        }
        protected override void Init() {
            base.Init();
            _dropdown = GetComponent<TMP_Dropdown>();
        }
        protected override void OnClickEventHandler(BaseEventData data = null)
        {
            _dropdown.Show();
        }
        private void OnEnable() {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }
        private void OnDisable() {
            LocalizationManager.LocalizationChanged -= Localize;
        }
        private void Localize()
        {
            _dropdown.captionText.font = LocalizationManager.GetFont();
            _dropdown.itemText.font = LocalizationManager.GetFont();
            for (var i = 0; i < LocalizationKeys.Count; i++)
            {
                _dropdown.options[i].text = LocalizationManager.Localize(LocalizationKeys[i]);
                
            }
            if (_dropdown.value < LocalizationKeys.Count)
            {
                _dropdown.captionText.text = LocalizationManager.Localize(LocalizationKeys[_dropdown.value]);
            }

        }
    }
}

