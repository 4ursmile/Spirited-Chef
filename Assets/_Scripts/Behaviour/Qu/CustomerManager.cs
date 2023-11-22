using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private CustomerMovePathSO _movePathSO;
    [SerializeField] private CustomerCharacter _customerPrefab;
    private Queue<CustomerCharacter> _customerQueue;
    private int _maxCapacity;
    private List<bool> _emptyPoint;
    private void Awake() {
        _maxCapacity = _movePathSO.InPath.Count;
        _emptyPoint = new List<bool>(_movePathSO.InPath.Count);
        _customerQueue = new Queue<CustomerCharacter>(_movePathSO.InPath.Count);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy() {
        StopAllCoroutines();
        UniversalObjectInstance.Instance.OrderCompleteAction -= CompleteOrder;
    }
    private void CompleteOrder(float score)
    {

    }
    public void NextCustomerCome()
    {
        if (_customerQueue.Count >= _maxCapacity)
            return;
        var customer = Instantiate(_customerPrefab);
        customer.transform.position = _movePathSO.InPath[0];
        customer.SetSprite(_movePathSO.CustomerSprites.GetRandom<Sprite>());
        _customerQueue.Enqueue(customer);
        int nextpoint = GetFirstEmptyIndex();
        _emptyPoint[nextpoint] = true;
        if (nextpoint == _maxCapacity -1)
            customer.GoTo(_movePathSO.InPath.Range(0, nextpoint+1),
            () =>
            {
                UniversalObjectInstance.Instance.NextOrder();
            });
        else
            customer.GoTo(_movePathSO.InPath.Range(0, nextpoint+1));
    }
    private int GetFirstEmptyIndex()
    {
        for (int i = _emptyPoint.Count-1; i>0; i--)
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
            if (nextpoint == _maxCapacity -1)
                customer.GoTo(_movePathSO.InPath.Range(0, nextpoint+1),
                () =>
                {
                    UniversalObjectInstance.Instance.NextOrder();
                });
            else
                customer.GoTo(_movePathSO.InPath.Range(0, nextpoint+1));

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
