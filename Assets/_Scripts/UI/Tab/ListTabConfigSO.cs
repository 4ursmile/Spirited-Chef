using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using ObjectS;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "ListTabConfig", menuName = "ScriptableObjects/UI/ListTabConfig", order = 1)]
    public class ListTabConfigSO : ScriptableObject
    {
        [SerializeField] private string _tabGroupName;
        public string TabGroupName => _tabGroupName;
        [SerializeField] List<TabItemObject> _tabItemObjects;
        public List<TabItemObject> TabItemObjects => _tabItemObjects;
    }
    [Serializable]
    public class TabItemObject
    {
        public string TabName;
        public Sprite TabIcon;
        public List<BaseFoodSO> FoodSOs;
    }
}

