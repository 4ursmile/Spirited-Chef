using System.Collections;
using System.Collections.Generic;
using Architecture;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
namespace UI
{
    public class SoundUIManager : MonoBehaviour
    {
        [field: SerializeField] public Slider BackGroundVolumeSlider { get; private set; }
        [field: SerializeField] public Slider SFXVolumeSlider { get; private set; }
        [field: SerializeField] public Slider JetVolumeSlider { get; private set; }
        [SerializeField] private AudioClip _onValueChangedClip;
        [SerializeField] private SoundManagerSO _soundManagerSO; 
        private void Start()
        {
            LoadVolume();
        }
        private void LoadVolume(int attempt = 0)
        {
            BackGroundVolumeSlider.onValueChanged.AddListener(OnChangeBackGroundVolume);
            SFXVolumeSlider.onValueChanged.AddListener(OnChangeSFXVolume);
            JetVolumeSlider.onValueChanged.AddListener(OnChangeJetVolume);
    
            _soundManagerSO.SetBackgroundVolume(BackGroundVolumeSlider.value, this);
            _soundManagerSO.SetSFXVolume(SFXVolumeSlider.value, this);


        }
        public void OnValueChanged()
        {
            if (_onValueChangedClip == null) return;
            _soundManagerSO.Play(_onValueChangedClip);
        }
        public void OnChangeBackGroundVolume(float volume)
        {
            if (_soundManagerSO == null) return;
            _soundManagerSO.SetBackgroundVolume(volume);
            OnValueChanged();
        }
        public void OnChangeSFXVolume(float volume)
        {
            if (_soundManagerSO == null) return;
            _soundManagerSO.SetSFXVolume(volume);
            OnValueChanged();
        }
        public void OnChangeJetVolume(float volume)
        {
            if (_soundManagerSO == null) return;
            OnValueChanged();
        }
        
    }
}

