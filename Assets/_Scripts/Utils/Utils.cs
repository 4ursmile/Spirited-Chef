using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using TMPro;
using DG.Tweening;
public static class Utils 
{
    public static Vector3 Clamp2D(Vector3 cv3, Vector2 widthLimit, Vector2 heightLimit) 
    {
        Vector3 res = new Vector3(
            Mathf.Clamp(cv3.x, widthLimit.x, widthLimit.y),
            cv3.y,
            Mathf.Clamp(cv3.z, heightLimit.x, heightLimit.y)
        );
        return res;
    } 
    public static Vector3 ElementWiseMultiply(this Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    public static void SetAlpha(this Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
    public static void SetAlpha(this SpriteRenderer sprite, float alpha)
    {
        Color c = sprite.color;
        c.a = alpha;
        sprite.color = c;
    }
    public static Vector3 ToVec3(this Vector2 v2) => new Vector3(v2.x, 0, v2.y);
    public static float MinMaxScale(this float value, float min, float max) 
    {
        if (value <= min) return 0;
        if (value >= max) return 1;
        return (value - min) / (max - min);
    }
        public static Vector2 Rotate(this Vector2 v2, float angle) => Quaternion.Euler(0, 0, angle) * v2;
    public static Vector2 MoveElipsePath(this Vector2 v2, float angle, float a, float b) => v2 + new Vector2(a * Mathf.Cos(angle), b * Mathf.Sin(angle));
    public static Vector2 MoveCirclePath(this Vector2 v2, float angle, float r) => v2 + new Vector2(r * Mathf.Cos(angle), r * Mathf.Sin(angle));
    public static Vector2 MoveElipsePath(this Vector2 v2, float angle, float a, float b, float rotation) => v2 + new Vector2(a * Mathf.Cos(angle), b * Mathf.Sin(angle)).Rotate(rotation);
    public static Vector3 MinusVec2(this Vector3 v3, Vector2 v2 ) => new Vector3(v3.x - v2.x, v3.y - v2.y, v3.z);
    public static Vector2 MinusVec2To2(this Vector3 v3, Vector2 v2) => new Vector2(v3.x - v2.x, v3.y - v2.y);
    public static Vector3 PlusVec2(this Vector3 v3, Vector2 v2 ) => new Vector3(v3.x + v2.x, v3.y + v2.y, v3.z);
    public static Vector3 PlusVec2To2(this Vector3 v3, Vector2 v2) => new Vector2(v3.x + v2.x, v3.y + v2.y);

    public static float DistanceScaleDown32(this Vector3 v3, Vector3 v2, float baseD) => v3.MinusVec2(v2).magnitude/baseD;
    public static Vector3 GoUp(this Transform trans, float distance) => trans.position + trans.up * distance; 
    public static Vector3 ToVector3(this Vector2 v2) => new Vector3(v2.x, v2.y, 0);
    public static List<T> GetRandomList<T>(this List<T> list, int number, bool unique = true, bool forceEnough = true) {
        List<T> result = new List<T>();
        if (list.Count < number && forceEnough)
        {
            unique = false;
        }
        if (unique)
        {
            if (number > list.Count)
            {
                number = list.Count;
            }
            List<T> tempList = new List<T>(list);
            for (int i = 0; i < number; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, tempList.Count);
                result.Add(tempList[randomIndex]);
                tempList.RemoveAt(randomIndex);
            }
        }
        else
        {
            for (int i = 0; i < number; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, list.Count);
                result.Add(list[randomIndex]);
            }
        }
        return result;
    }
    public static Vector2 ToVector3(this Vector3 v3) => new Vector2(v3.x, v3.y);
    public static Vector3 RotateDirection(this Vector3 v3, float direction) => Quaternion.Euler(0, 0, direction) * v3;

    public static void DestroyAllChild(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public delegate void EventAction(params object[] senders);
    public static string FromSecondsToTimeFormat(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"mm\:ss");
    }
    public static void DOAlphaText(this TextMeshProUGUI text, float alpha, float duration)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1-alpha);
        text.DOFade(alpha, duration).SetEase(Ease.InOutSine);
        text.rectTransform.DOShakeScale(duration, 0.1f, 10, 90, false);
    }
    // calculate dot bettween 2 list float
    public static float Dot(this List<float> list1, List<float> list2)
    {
        float result = 0;
        for (int i = 0; i < list1.Count; i++)
        {
            result += list1[i] * list2[i];
        }
        return result;
    }
    // calculate lentght of list float
    public static float SqrMattidue(this List<float> list)
    {
        float result = 0;
        for (int i = 0; i < list.Count; i++)
        {
            result += list[i] * list[i];
        }
        return result;
    }
    public static float SqrtMattidue(this List<float> list)
    {
        float result = list.SqrMattidue();
        return Mathf.Sqrt(result);
    } 
    // calculate cosine bettween 2 list float
    public static float CosineSim(this List<float> list1, List<float> list2)
    {
        return list1.Dot(list2) / (list1.SqrMattidue() * list2.SqrMattidue());
    }
    public static T GetRandom<T>(this List<T> ts) 
    {
        if (ts.Count == 0)
            return default;
        int index = UnityEngine.Random.Range(0, ts.Count-1);
        return ts[index];
    }
    public static Coroutine Start(this IEnumerator coroutine, MonoBehaviour monoBehaviour) => monoBehaviour.StartCoroutine(coroutine);
}
public static class Helper
{
    public static readonly Dictionary<float, UniTask> waitForSecondsDic = new Dictionary<float, UniTask>();
    public static UniTask GetWaitForSecond(float time)
    {
        if (waitForSecondsDic.ContainsKey(time))
        {
            return waitForSecondsDic[time];
        }
        else
        {
            waitForSecondsDic.Add(time, UniTask.WaitForSeconds(time));
            return waitForSecondsDic[time];
        }
    }
    public static bool BinaryRandom(int chance = 50) => UnityEngine.Random.Range(0, 100) <= chance;
    public static T LoadAsync<T>(string tableName, string key, AsyncOperationHandle<T> handle, int attemp = 0)
    {
        if (attemp > 3) return default;
        handle = Database.Table(tableName).GetAsync<T>(key);
        T result = default;
        handle.Completed += (res) => {
            if (res.Status == AsyncOperationStatus.Succeeded)
            {
                result = res.Result;
                return;
            }
            else
            {
                result = LoadAsync<T>(tableName, key, handle, attemp + 1);
            }
        };
        return result;
    }
    // Index for foreach loop
    public static IEnumerable<(T item, int index)> Enumerate<T>(this IEnumerable<T> enumerable)
    {
        int index = 0;
        foreach (var item in enumerable)
        {
            yield return (item, index);
            index++;
        }
    }
    public static Vector3[] Range(this List<Vector3> vector3s, int start, int end)
    {
        if (start < 0 || end > vector3s.Count)
            return null;
        if (start > end)
            return vector3s.GetRange(start, 1).ToArray();
        return vector3s.GetRange(start, end - start).ToArray();
    }
    public static float GetDistanceScale(this Vector3 vs, Vector3 vd, float maxDistance) => Vector3.Distance(vs, vd) / maxDistance;
    public static Dictionary<float, WaitForSeconds> ForSecondDict = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (ForSecondDict.ContainsKey(time))
            return ForSecondDict[time];
        else
        {
            ForSecondDict.Add(time, new WaitForSeconds(time));
            return ForSecondDict[time];
        }
    }
}

public class QueuePool<T> 
{
    Queue<T> _pool;
    int _maxSize;
    int _countAll;
    public int CountAll => _countAll;
    public int CountActive => _countAll - _pool.Count;
    public int CountInactive => _pool.Count;
    private bool _doubleChecked;
    public void SetDoubleChecked(bool doubleChecked) => _doubleChecked = doubleChecked;
    Action<T> _get;
    Action<T> _release;
    Action<T> _destroy;
    Func<T> _create;

    public QueuePool( System.Func<T> create, System.Action<T> get, System.Action<T> release, System.Action<T> destroy,bool doubleChecked, int initSize, int maxSize)
    {
        _pool = new Queue<T>(initSize);
        _maxSize = maxSize;
        _create = create;
        _countAll = 0;
        _get = get;
        _release = release;
        _destroy = destroy;
        _doubleChecked = doubleChecked;

    }
    public T Get()
    {
        if (_pool.Count > 0)
        {
            T sample = _pool.Dequeue();
            _get(sample);
            return sample;
        }
        if (_countAll < _maxSize)
        {
            T sample = _create();
            _get(sample);
            _countAll++;
            return sample;
        }
        return default;
    }
    public void Release(T sample)
    {
        if (_pool.Count < _maxSize)
        {
            if (_doubleChecked)
                if (_pool.Contains(sample))
                {
                    return;
                }

            _release(sample);
            _pool.Enqueue(sample);
        }
        else
        {
            _countAll--;
            _destroy(sample);
        }
    }
    public void OnDestroy()
    {
        while (_pool.Count > 0)
        {
            _countAll--;
            _destroy(_pool.Dequeue());
        }
    }
    public void Clear() => OnDestroy();
    public void Dispose() => OnDestroy();

    public void DestroyHalf()
    {
        int half = _countAll / 2;
        while (_pool.Count > half)
        {
            _countAll--;
            _destroy(_pool.Dequeue());
        }
    }
    public void WarmUp(int step)
    {
        for (int i = 0; i < step; i++)
        {
            _pool.Enqueue(_create());
            _countAll++;
        }
    }
    public async UniTaskVoid WarmAsync(int step)
    {
        await UniTask.RunOnThreadPool(() => WarmUp(step));
    }
}
public class LinkedListPool<T>
{
    LinkedList<T> _pool;
    public LinkedList<T> Pool => _pool;
    int _maxSize;
    int _countAll;
    int _countActive;
    bool _doubleChecked;
    public int CountAll => _countAll;
    public int CountActive => _countActive;
    public int CountInactive => _countAll - _countActive;
    Action<T> _get;
    Action<T> _release;
    Action<T> _destroy;
    Func<T> _create;
    public LinkedListPool(System.Func<T> create, System.Action<T> get, System.Action<T> release, System.Action<T> destroy, bool doubleChecked, int initSize, int maxSize)
    {
        _pool = new LinkedList<T>();
        _maxSize = maxSize;
        _create = create;
        _countAll = 0;
        _countActive = 0;
        _get = get;
        _release = release;
        _destroy = destroy;
        _doubleChecked = doubleChecked;
    }
    public T Get()
    {
        if (CountInactive > 0)
        {
            T sample = _pool.First.Value;
            _pool.RemoveFirst();
            _get(sample);
            _countActive++;
            return sample;
        }
        if (_countAll < _maxSize)
        {
            T sample = _create();
            _get(sample);
            _countAll++;
            _countActive++;
            return sample;
        }
        return default;
    }   
    public void Release(T sample)
    {
        if (_pool.Count < _maxSize)
        {
            if (_doubleChecked)
                if (_pool.Contains(sample))
                {
                    return;
                }

            _release(sample);
            _pool.AddLast(sample);
            _countActive--;
        }
        else
        {
            _countAll--;
            _destroy(sample);
        }
    }
    public void OnDestroy()
    {
        while (_pool.Count > 0)
        {
            _countAll--;
            _destroy(_pool.First.Value);
            _pool.RemoveFirst();
        }
    }
    public void Clear() => OnDestroy();
    public void Dispose() => OnDestroy();
    public void DestroyHalf()
    {
        int half = _countAll / 2;
        while (_pool.Count > half)
        {
            _countAll--;
            _destroy(_pool.First.Value);
            _pool.RemoveFirst();
        }
    }
    public void WarmUp(int step)
    {
        for (int i = 0; i < step; i++)
        {
            _pool.AddLast(_create());
            _countAll++;
        }
    }
    public async UniTaskVoid WarmAsync(int step)
    {
        await UniTask.RunOnThreadPool(() => WarmUp(step));
    }
}
