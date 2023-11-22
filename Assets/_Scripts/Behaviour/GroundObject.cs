using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class GroundObject : MonoBehaviour, IClickable
    {
        public Vector3 Position => transform.position;
        public ObjectType Type => ObjectType.Ground;
        public void OnClick()
        {
            Debug.Log("Ground Clicked");
        }
    }
}

