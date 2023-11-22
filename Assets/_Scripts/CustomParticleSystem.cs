using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class CustomParticleSystem : MonoBehaviour
{
    ParticleSystem _particleSystem;
    public ParticleSystem ParticleSystem => _particleSystem;
    private float _duration;
    public float Duration => _duration;
    public Action<CustomParticleSystem> OnParticleSystemStoppedCallback;
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.None;
        _duration = main.duration;
    }
    public void Play()
    {
        CancelInvoke();
        _particleSystem.Play();
        Invoke("Stop", _duration);
    }
    public void Play(float duration)
    {
        CancelInvoke();
        _particleSystem.Play();
        Invoke("Stop", duration);
    }
    public void Stop()
    {
        _particleSystem.Stop();
        OnParticleSystemStoppedCallback?.Invoke(this);
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void SetParent(Transform parent, bool worldPositionStays)
    {
        transform.SetParent(parent, worldPositionStays);
    }
    public void SetParentAndResetLocal(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
    }

}
