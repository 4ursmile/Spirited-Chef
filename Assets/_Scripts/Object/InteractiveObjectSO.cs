using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ObjectS
{
    [CreateAssetMenu(fileName = "InteractiveObjectSO", menuName = "ScriptableObjects/Object/InteractiveObjectSO", order = 1)]
    public class InteractiveObjectSO : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] string _description;
        [SerializeField] Vector3 _positionOffset;
        [SerializeField] bool _canOverlap = false;
        [SerializeField] bool _requireFocus = false;
        public string Name => _name;
        public string Description => _description;
        public Vector3 PositionOffset => _positionOffset;
        public bool CanOverlap => _canOverlap;
        public bool RequireFocus => _requireFocus;
    }
}

