using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using UnityEngine.AddressableAssets;
using System;
using UI;
using Cysharp.Threading.Tasks;
namespace Architecture
{
    [CreateAssetMenu(fileName = "SoundManagerSO", menuName = "ScriptableObjects/Architecture/SoundManagerSO", order = 1)]
    public class SoundManagerSO : ScriptableObject
    {
        public AudioSource BackgroundSource { get; private set; }

        public AudioSource SFXSource { get; private set; }
        private LinkedListPool<AudioSource> _sfxSourcePool;
        public int SFXCurrentIndex { get; private set; }
        public void Inittial(AudioSource bg, AudioSource sfx)
        {
            BackgroundSource = bg; 
            SFXSource = sfx;
            InitPool();
        }
        public void InitPool()
        {
            _sfxSourcePool = new LinkedListPool<AudioSource>(
                () => { var a = Instantiate(SFXSource); a.gameObject.SetActive(false); return a; },
                (source) => source.gameObject.SetActive(true),
                (source) => source.gameObject.SetActive(false),
                (source) => Destroy(source.gameObject),
                false, 5, 5);
        }
        public async void RentAudioSource (AudioClip clip, float time)  {
            var source = _sfxSourcePool.Get();
            if (source == null)
            {
                return;
            }
            source.volume = SFXSource.volume;
            source.clip = clip;
            source.loop = true;
            source.Play();
            await UniTask.WaitForSeconds(time);
            source.Stop();
            source.loop = false;
            _sfxSourcePool.Release(source);
        }
        public AudioSource RentAudioSource()
        {
            var source = _sfxSourcePool.Get();
            if (source == null)
            {
                var a = Instantiate(SFXSource);
                a.volume = SFXSource.volume;
                return a;
            }
            source.volume = SFXSource.volume;
            return source;
        }
        public void ReleaseAudioSource(AudioSource audioSource)
        {
            _sfxSourcePool.Release(audioSource);
        }
        public void SetBackgroundVolume(float volume, SoundUIManager soundUIManager = null)
        {
            if (BackgroundSource == null) return;
            if (soundUIManager != null)
            {
                soundUIManager.BackGroundVolumeSlider.value = BackgroundSource.volume;
                return;
            }

            BackgroundSource.volume = volume;
            PlayerPrefs.SetFloat("bgVolume", volume);
            if (volume == 0 && BackgroundSource.gameObject.activeSelf == true)
            {
                BackgroundSource.Stop();
                BackgroundSource.gameObject.SetActive(false);
                
            } else if (BackgroundSource.gameObject.activeSelf == false && volume > 0)
            {
                BackgroundSource.gameObject.SetActive(true);
                BackgroundSource.Play();
            }
        }
        public void SetSFXVolume(float volume, SoundUIManager soundUIManager = null)
        {
            if (SFXSource == null) return;
            if (soundUIManager != null)
            {
                soundUIManager.SFXVolumeSlider.value = SFXSource.volume;
                return;
            }
            SFXSource.volume = volume;
            PlayerPrefs.SetFloat("sfxVolume", volume);
            foreach (var source in _sfxSourcePool.Pool)
            {
                source.volume = volume;
            }
        }
        public void Play(AudioClip audioClip)
        {
            SFXSource.PlayOneShot(audioClip);
        }
        public void Play(AudioClip audioClip, float volume)
        {
            SFXSource.PlayOneShot(audioClip, volume);
        }
        public void Play(AudioClip audioClip, float volume, float pan)
        {
            if (SFXSource == null) return;
            SFXSource.panStereo = pan;
            SFXSource.PlayOneShot(audioClip, volume);

        }
        public void Play(AudioClip audioClip, Vector2 virtualPos)
        {
            
            SFXSource.panStereo = Mathf.Clamp(virtualPos.x, -1, 1);
            SFXSource.PlayOneShot(audioClip, Mathf.Clamp(virtualPos.magnitude, 0, 1));
        }
        public void PlayBackGroundMusic()
        {
            BackgroundSource.gameObject.SetActive(true);
            BackgroundSource.Play();
        }
        public void PlayBackGroundMusic(AudioClip backGroundMusic)
        {
            BackgroundSource.gameObject.SetActive(true);
            BackgroundSource.clip = backGroundMusic;
            BackgroundSource.Play();
        }
        
    }
    
}

