using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Manager;
using UnityEngine;
using UnityEngine.UI;


public class LocalizationRuntime : MonoBehaviour
{
    [SerializeField] private LanguageManagerSO _languageManagerSO;
    void Start()
    {
        LocalizationManager.Read(_languageManagerSO);
        _languageManagerSO.Init(this);
    }
    public void SetLocalization(string localization)
    {
        LocalizationManager.Language = localization;
        PlayerPrefs.SetString("Language", localization);
    }
}
