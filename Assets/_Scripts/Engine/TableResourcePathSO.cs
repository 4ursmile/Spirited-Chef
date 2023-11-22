using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
namespace Resource
{
    [CreateAssetMenu(fileName = "Table", menuName = "ScriptableObjects/Database/table", order = 1)]
    public class TableResourcePathSO : ScriptableObject
    {
        [field: SerializeField]
        public SerializedDictionary<string, string> _tablePathDictionary { get; private set; }

        public string Get (string key) => _tablePathDictionary.ContainsKey(key) ? _tablePathDictionary[key] : null;
        public AsyncOperationHandle<T> GetAsync<T> (string key) => Addressables.LoadAssetAsync<T>(_tablePathDictionary[key]);
        public void GetAsync<T> (string key, System.Action<AsyncOperationHandle<T>> callback) => Addressables.LoadAssetAsync<T>(_tablePathDictionary[key]).Completed += callback;
        public T GetResult<T> (string key) => Addressables.LoadAssetAsync<T>(_tablePathDictionary[key]).Result;
        public void Set (string key, string value) => _tablePathDictionary[key] = value;
        public void Remove (string key) => _tablePathDictionary.Remove(key);
        public void UpdateRecord (string key, string value)
        {
            if (_tablePathDictionary.ContainsKey(key)) _tablePathDictionary[key] = value; 
        }


    }
}

