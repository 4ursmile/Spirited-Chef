using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class LocalizationDropDownTMPLanguage : LocalizationDropDownTMP
    {
        [field: SerializeField] public LocalizationRuntime LocalizationRuntime { get; private set; }
        public void OnOptionChanged(int index)
        {
            LocalizationRuntime.SetLocalization(LocalizationKeys[index]);
        }
    }
}

