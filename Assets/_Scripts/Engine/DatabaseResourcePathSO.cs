using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
namespace Resource
{
    [CreateAssetMenu(fileName = "Database", menuName = "ScriptableObjects/Database/database")]
    public class DatabseResourcePathSO : ScriptableObject
    {
        
        [field: SerializeField] public SerializedDictionary<string, TableResourcePathSO> ResourceTables { get; private set; }

        public TableResourcePathSO GetTable(string key) => ResourceTables.ContainsKey(key) ? ResourceTables[key]:null;
        public void AddTable(string key, TableResourcePathSO value) => ResourceTables.Add(key, value);
        public void RemoveTable(string key) => ResourceTables.Remove(key);
        public void UpdateTable(string key, TableResourcePathSO value)
        {
            if (ResourceTables.ContainsKey(key))
            {
                ResourceTables[key] = value;
            }
        }
    }
}

