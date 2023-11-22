using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using Assets.SimpleLocalization.Scripts;
namespace ObjectS
{
    public class WarningText : MonoBehaviour
    {
        [SerializeField] TextMeshPro _text;
        [SerializeField] float FadeTime;
        [SerializeField] Vector3 _offset;
        public void Set(string Text, Action<WarningText> OnParticleSystemStoppedCallback = null, bool dontUseDic = false)
        {

            _text.text = dontUseDic? Text: LocalizationManager.Localize(Text);
            _text.font = LocalizationManager.GetFont();
            _text.DOFade(1, 0);
            Sequence sequence = BuildSequence();
            sequence.Play().OnComplete(() => OnParticleSystemStoppedCallback?.Invoke(this));
        }
        public Sequence BuildSequence()
        {
            float firstPhase = FadeTime / 3;
            float secondPhase = FadeTime - firstPhase;

            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, transform.DOScale(1.5f, firstPhase).SetEase(Ease.OutBounce));
            sequence.Insert(firstPhase, transform.DOMove(transform.position + Vector3.down*0.5f, secondPhase));
            sequence.Insert(firstPhase, transform.DOScale(1, secondPhase).SetEase(Ease.InOutCubic));
            sequence.Insert(firstPhase, _text.DOFade(0, secondPhase));
            return sequence;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position + _offset;
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position+_offset, rotation);
        }
        public void SetScale(Vector3 scale)
        {
            transform.localScale = scale;
        }
        public void SetPositionAndRotationAndScale(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.SetPositionAndRotation(position + _offset, rotation);
            transform.localScale = scale;
        }
    }
}

