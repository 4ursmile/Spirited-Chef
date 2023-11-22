using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Architecture;
using Resource;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Manager
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundManagerSO _soundManagerSO;
        [SerializeField] private AudioSource _backgroundSource;
        [SerializeField] private AudioSource _sfxSource;
        private void Awake()
        {
            _soundManagerSO.Inittial(_backgroundSource, _sfxSource);
            
        }

    }
}

