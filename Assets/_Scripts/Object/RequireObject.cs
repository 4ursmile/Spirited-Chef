using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ObjectS
{
    public class RequireObject : MonoBehaviour
    {
        [SerializeField] RequireObjectType _requireObjectType;
        [SerializeField] private Vector3 _foodOffset; 
        public RequireObjectType RequireObjectType => _requireObjectType;

        // Start is called before the first frame update
    }
    public enum RequireObjectType
    {
        None,
        To,
        Chen,
        Dia,
        Thau,
        Rac
    }
}

