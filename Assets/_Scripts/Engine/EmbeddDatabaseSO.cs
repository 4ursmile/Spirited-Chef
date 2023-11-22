using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EmbeddDatabase", menuName = "ScriptableObjects/Database/EmbeddDatabase", order = 1)]
    public class EmbeddDatabaseSO : ScriptableObject
    {
        [SerializeField] private TextAsset _embeddDatabase;
        public string Get () => _embeddDatabase.text;
        private Dictionary<string, List<float>> _jObject;
        public Dictionary<string, List<float>> JObject => _jObject;
        public void Init()
        {
            _jObject = JsonConvert.DeserializeObject<Dictionary<string, List<float>>>(_embeddDatabase.text);
        }
        public async void InitAsync(Action callbacks = null)
        {
            _jObject = JsonConvert.DeserializeObject<Dictionary<string, List<float>>>(_embeddDatabase.text);
            await UniTask.Yield();
            callbacks?.Invoke();
        }
        public List<float> QueryEmbedd(string key)
        {
            List<float> result = _jObject[key];
            return result;
        }
        public float Score (string key, string key2)
        {
            var vec1 = _jObject[key];
            var vec2 = _jObject[key2];
            float cosinSim = vec1.CosineSim(vec2);
            return cosinSim;
        }
        public async UniTask<float> ScoreAsync (string key, string key2, Action<float> callback = null)
        {
            var vec1 = _jObject[key];
            var vec2 = _jObject[key2];
            float cosinSim = vec1.CosineSim(vec2);
            await UniTask.Yield();
            callback?.Invoke(cosinSim);
            return await UniTask.FromResult(cosinSim);
        }
    }
}

