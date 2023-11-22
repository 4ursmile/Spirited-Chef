using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UnityEngine.UI;
using Architecture;
using TMPro;
namespace Manager
{
    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] private LocalizationDropDownTMP _localizationDropDownTMP;
        [SerializeField] private LanguageManagerSO _languageManagerSO;
        [SerializeField] private AudioClip _onValueChangedClip;
        [SerializeField] private SoundManagerSO _soundManagerSO;
        private void Awake() {
            _localizationDropDownTMP.Dropdown.options.Clear();
            foreach (var language in _languageManagerSO.LanguageLists)
            {
                _localizationDropDownTMP.Dropdown.options.Add(new TMP_Dropdown.OptionData(language.ToString()));
            }
            _localizationDropDownTMP.LocalizationKeys = _languageManagerSO.LanguageLists;
            _localizationDropDownTMP.Dropdown.onValueChanged.AddListener(OnValueChanged);
            
        }
        
        private void Start() {
            _languageManagerSO.SetLanguage(_localizationDropDownTMP.Dropdown.value, _localizationDropDownTMP.Dropdown);
           // _localizationDropDownTMP.Dropdown.onValueChanged.AddListener(OnValueChanged);
        }
        private void OnValueChanged(int index)
        {
            _languageManagerSO.SetLanguage(index);
            if (_onValueChangedClip == null) return;
            _soundManagerSO.Play(_onValueChangedClip);
        }
    }
}

