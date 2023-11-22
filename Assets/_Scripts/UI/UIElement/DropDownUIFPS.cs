using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using TMPro;
using Architecture;

namespace UI
{
    public class DropDownUIFPS : MonoBehaviour
    {
        [SerializeField] private FPSManagerSO _fpsManagerSO;
        [SerializeField] private AudioClip _onValueChangedClip;
        [SerializeField] private SoundManagerSO _soundManagerSO;
        [SerializeField] private TMP_Dropdown _dropdown;
        private void Awake() {
            _dropdown.options.Clear();
            foreach (var fps in _fpsManagerSO.FPSLists)
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData(fps.ToString()));
            }
            _fpsManagerSO.Init();
            _fpsManagerSO.SetFPS(_fpsManagerSO.CurrentFPSIndex, _dropdown);
            _dropdown.onValueChanged.AddListener(OnValueChanged);
        }
        private void OnValueChanged(int index)
        {
            _fpsManagerSO.SetFPS(index);
            if (_onValueChangedClip == null) return;
            _soundManagerSO.Play(_onValueChangedClip);
        }
    }
}

