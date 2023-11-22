using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Manager
{
    [CreateAssetMenu(fileName = "LanguageManagerSO", menuName = "ScriptableObjects/Manager/LanguageManagerSO", order = 1)]
    public class LanguageManagerSO : ScriptableObject
    {
        [field: SerializeField] public List<string> LanguageLists { get; private set; }
        [field: SerializeField] public List<TMP_FontAsset> FontLists { get; private set; }
        public int CurrentLanguageIndex { get; private set; } = 0;
        public string CurrentLanguage => LanguageLists[CurrentLanguageIndex];
        public LocalizationRuntime LocalizationRuntime { get; private set; }
        public void Init(LocalizationRuntime localizationRuntime)
        {
            LocalizationRuntime = localizationRuntime;
            CurrentLanguageIndex = 0;
            if (PlayerPrefs.HasKey("Language"))
            {
                CurrentLanguageIndex = PlayerPrefs.GetInt("Language", 0);
            } else
            {
                int index = LanguageLists.IndexOf(Application.systemLanguage.ToString());
                if (index != -1)
                {
                    CurrentLanguageIndex = index;
                }
            }
        
            
        }
        public void SetLanguage(int index, TMP_Dropdown dropdown = null)
        {
            if (dropdown != null)
            {
                dropdown.value = CurrentLanguageIndex;
                return;
            }
            CurrentLanguageIndex = index;
            PlayerPrefs.SetInt("Language", index);         
            LocalizationRuntime.SetLocalization(CurrentLanguage);
        }
        public TMP_FontAsset GetFont(int index) => FontLists[index];
        public TMP_FontAsset GetFont() => FontLists[CurrentLanguageIndex];
    }
}
