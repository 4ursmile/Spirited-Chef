using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] SoundManagerSO _soundManagerSO;
    [SerializeField] private AudioClip _onValueChangedClip;
    [SerializeField] private bool isBG;
    private Slider _volumeSlider;
    private void Awake() {
        _volumeSlider = GetComponent<Slider>();
        _volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }
    private void OnEnable() {
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }
    private void OnDisable() {
        _volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }
    private void OnVolumeChanged(float volume) {
        if (isBG) {
            _soundManagerSO.SetBackgroundVolume(volume);
        } else {
            _soundManagerSO.SetSFXVolume(volume);
        }
        _soundManagerSO.Play(_onValueChangedClip);

    }

}
