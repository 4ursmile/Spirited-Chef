using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomBehaviour 
{
    private List<int> _sumStackArray;
    public RandomBehaviour(List<int> weightList)
    {
        _sumStackArray = new List<int>();
        int sum = 0;
        for (int i = 0; i < weightList.Count; i++)
        {
            sum += weightList[i];
            _sumStackArray.Add(sum);
        }
    }
    public void RemoveAt(int index)
    {
        int indexWeight = index>0?_sumStackArray[index]- _sumStackArray[index - 1]: _sumStackArray[index];
        for (int i = index+1; i < _sumStackArray.Count; i++)
        {
            _sumStackArray[i] -= indexWeight;
        }
        _sumStackArray.RemoveAt(index);
    }
    public void UpdateWeight(int index, int weight)
    {
        int indexWeight = index > 0 ? _sumStackArray[index] - _sumStackArray[index - 1] : _sumStackArray[index];
        int delta = weight - indexWeight;
        for (int i = index; i < _sumStackArray.Count; i++)
        {
            _sumStackArray[i] += delta;
        }
    }
    public void AddAt(int index, int weight)
    {
        int indexWeight = index > 0 ? _sumStackArray[index - 1] + weight : weight;
        _sumStackArray.Insert(index, indexWeight);
        for (int i = index+1; i < _sumStackArray.Count; i++)
        {
            _sumStackArray[i] += weight;
        }
    }
    public void Update(List<int> weightList)
    {
        _sumStackArray.Clear();
        int sum = 0;
        for (int i = 0; i < weightList.Count; i++)
        {
            sum += weightList[i];
            _sumStackArray.Add(sum);
        }
    }
    public int GetRandomIndex()
    {
        int random = Random.Range(0, _sumStackArray[_sumStackArray.Count - 1]);
        for (int i = 0; i < _sumStackArray.Count; i++)
        {
            if (random < _sumStackArray[i])
            {
                return i;
            }
        }
        return _sumStackArray.Count - 1;
    }
}
