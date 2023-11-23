using System.Collections;
using System.Collections.Generic;
using Character;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
public class CustomerManager : MonoBehaviour
{
    [SerializeField] private CustomerMovePathSO _movePathSO;
    [SerializeField] private CustomerCharacter _customerPrefab;
    private Queue<CustomerCharacter> _customerQueue;
    private int _maxCapacity;
    private List<bool> _emptyPoint;
    private void Awake() {
        _maxCapacity = _movePathSO.InPath.Count;
        _emptyPoint = new List<bool>(_maxCapacity);
        for (int i = 0; i < _maxCapacity; i++)
        {
            _emptyPoint.Add(false);
        }
        _customerQueue = new Queue<CustomerCharacter>(_maxCapacity);
    }
    private void Start() {
        StartCoroutine(CustomerComeIn());
        UniversalObjectInstance.Instance.OrderCompletedAction += CompleteOrder;
    }
    private void OnDestroy() {
        StopAllCoroutines();
        UniversalObjectInstance.Instance.OrderCompletedAction -= CompleteOrder;
    }
    private async void CompleteOrder(float score)
    {
        _emptyPoint[_maxCapacity-1] = false;
        var cus = _customerQueue.Dequeue();

        cus.GetOrder(score);
        await UniTask.WaitForSeconds(1f);
        cus.GoTo(_movePathSO.OutPath.ToArray(), 0 , () =>
        {
            Destroy(cus.gameObject);
        });
        UpdateQueue();
    }
    public void NextCustomerCome()
    {
        if (_customerQueue.Count >= _maxCapacity)
            return;
        var customer = Instantiate(_customerPrefab, _movePathSO.InPath[0], _customerPrefab.transform.rotation);
        customer.SetSprite(_movePathSO.CustomerSprites.GetRandom<Sprite>());
        _customerQueue.Enqueue(customer);
        int nextpoint = GetFirstEmptyIndex();
        _emptyPoint[nextpoint] = true;
        if (nextpoint == _maxCapacity -1)
            customer.GoTo(_movePathSO.InPath.Range(0, nextpoint+1), nextpoint,
            () =>
            {
                UniversalObjectInstance.Instance.NextOrder();
            });
        else
        {
            customer.transform.DOKill();
            customer.GoTo(_movePathSO.InPath.Range(0, nextpoint+1), nextpoint);
        }
    }
    private int GetFirstEmptyIndex()
    {
        for (int i = _maxCapacity-1; i>0; i--)
        {
            if (!_emptyPoint[i])
                return i;
        }
        return 0;
    }
    public void UpdateQueue()
    {
        foreach(CustomerCharacter customer in _customerQueue)
        {
            int nextpoint = GetFirstEmptyIndex();
            _emptyPoint[nextpoint] = true;
            if (nextpoint >0)
                _emptyPoint[nextpoint-1] = false;
            if (nextpoint == _maxCapacity -1)
                customer.GoTo(_movePathSO.InPath.Range(customer.LastIndex, nextpoint+1), nextpoint,
                () =>
                {
                    UniversalObjectInstance.Instance.NextOrder();
                });
            else
                customer.GoTo(_movePathSO.InPath.Range(customer.LastIndex, nextpoint+1), nextpoint);

        }
    }
    IEnumerator CustomerComeIn()
    {
        while (true)
        {
            yield return Helper.GetWaitForSeconds(UniversalObjectInstance.Instance.GetWaitTime);
            NextCustomerCome();
        }
    }
}
