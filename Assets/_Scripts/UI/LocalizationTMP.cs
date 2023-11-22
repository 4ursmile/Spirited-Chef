using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;
using TMPro;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationTMP : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        [field: SerializeField] public string LocalizationKey { get; set; }
        private void Awake() {
            _text = GetComponent<TextMeshProUGUI>();
        }
        public void Start()
        {
            OnLocalChanged();
            LocalizationManager.LocalizationChanged += OnLocalChanged;

        }
        protected virtual void OnLocalChanged()
        {

            _text.text = LocalizationManager.Localize(LocalizationKey);
            _text.font = LocalizationManager.GetFont();
        }
        private void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= OnLocalChanged;
        }
    }
}

