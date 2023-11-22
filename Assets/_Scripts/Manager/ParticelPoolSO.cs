using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(fileName = "ParticelPool", menuName = "ScriptableObjects/VFX/ParticelPool")]
    public class ParticelPoolSO : ScriptableObject
    {
        private LinkedListPool<CustomParticleSystem> _particlePool;
        public LinkedListPool<CustomParticleSystem> Pool => _particlePool;
        [SerializeField] private int _initSize;
        [SerializeField] private int _maxSize;
        [SerializeField] private CustomParticleSystem _particleSystem;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private bool _makeSound;
        [SerializeField] private bool _oneShotSound = true;
        [SerializeField] [Range(0, 1)] private float _volume = 0.5f;
        public bool OneShotSound => _oneShotSound;
        private CustomParticleSystem _emergencyParticle;
        private SoundManagerSO _soundManagerSO;
        public float ScaleMultiplier = 2;
        public void Init(SoundManagerSO soundManagerSO)
        {
            _soundManagerSO = soundManagerSO;
            _particlePool = new LinkedListPool<CustomParticleSystem>(
                () => { var a = Instantiate(_particleSystem); 
                        a.OnParticleSystemStoppedCallback = Release2Pool;  
                        a.gameObject.SetActive(false); 
                        return a;
                    }, 
                (particle) => particle.gameObject.SetActive(true), 
                (particle) => particle.gameObject.SetActive(false), 
                (particle) => Destroy(particle.gameObject), 
                false, _initSize, _maxSize);
            _emergencyParticle = Instantiate(_particleSystem);
            _emergencyParticle.gameObject.SetActive(false);
            _emergencyParticle.OnParticleSystemStoppedCallback = (a)=> {a.gameObject.SetActive(false);};
        }
        void Release2Pool(CustomParticleSystem particleSystem)
        {
            _particlePool.Release(particleSystem);
        }
        public CustomParticleSystem GetVFX()
        {
            var particle = _particlePool.Get();
            if (_makeSound)
                _soundManagerSO.Play(_audioClip, _volume);
            if (particle == null)
            { 
                _emergencyParticle.gameObject.SetActive(true);
                return _emergencyParticle;
            }
            return particle;
        }
        public CustomParticleSystem Get => GetVFX();

    }
}

