using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Resource
{
    [CreateAssetMenu(fileName = "BackGroundSoundSO", menuName = "ScriptableObjects/BackGroundSoundSO", order = 1)]
    public class BackGroundSoundSO : ScriptableObject
    {
        [field:SerializeField] public AudioClip BGClip { get; private set; }
        [field:SerializeField] public AudioClip AmbianceClip { get; private set; }
    }
}

