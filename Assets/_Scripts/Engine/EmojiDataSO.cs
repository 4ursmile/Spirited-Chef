using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
[CreateAssetMenu(fileName = "EmojiData", menuName = "ScriptableObjects/EmojiData", order = 1)]
public class EmojiDataSO : ScriptableObject
{
    [SerializeField] private SerializedDictionary<string, Sprite> _emojiData;
    public Sprite Get(string key) => _emojiData.ContainsKey(key) ? _emojiData[key] : null;
    public void Set(string key, Sprite value) => _emojiData[key] = value;
    public void Remove(string key) => _emojiData.Remove(key);
    public Sprite GetRandom() => _emojiData.ElementAt(Random.Range(0, _emojiData.Count)).Value;
    public void UpdateRecord(string key, Sprite value)
    {
        if (_emojiData.ContainsKey(key)) _emojiData[key] = value;
    }
}
