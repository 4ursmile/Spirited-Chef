using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(fileName = "FPSManagerSO", menuName = "ScriptableObjects/Manager/FPSManagerSO", order = 1)]
    public class FPSManagerSO : ScriptableObject
    {
        [field: SerializeField] public int[] FPSLists { get; private set; }
        public int CurrentFPSIndex { get; private set; } = 1;
        public int CurrentFPS => FPSLists[CurrentFPSIndex];
        public void Init()
        {
            CurrentFPSIndex = PlayerPrefs.GetInt("FPS", 1);
            SetFPS(CurrentFPSIndex);
        }
        public void SetFPS(int index, TMP_Dropdown dropdown = null)
        {
            if (dropdown != null)
            {
                dropdown.value = CurrentFPSIndex;
                return;
            }
            CurrentFPSIndex = index;
            PlayerPrefs.SetInt("FPS", index);
#if !UNITY_EDITOR
            Application.targetFrameRate = CurrentFPS;
#else
            Debug.Log("Current FPS: " + CurrentFPS);
#endif
        }
        
    }
}
