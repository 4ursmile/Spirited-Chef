using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Architecture;
namespace UI
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] private InGameUIBridgeSO _bridge;
        [SerializeField] private UIFollow _selectedCircle;
        private void Awake() {
            _bridge.Init(this);
            
        }
        private void Start() {
            _bridge.SetSelectedImage(_selectedCircle);
        }

    }
}

