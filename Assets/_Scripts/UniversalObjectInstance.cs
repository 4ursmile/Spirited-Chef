using System.Collections;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;
using ObjectS;
using Architecture;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using System;
public class UniversalObjectInstance : MonoBehaviour
{
    public static UniversalObjectInstance Instance;
    [SerializeField] private List<CharacterControllerS> _characterControllerS;
    [SerializeField] private List<InteractiveObject> _inActiveInteractiveObjects;
    [SerializeField] private List<int> _numberOrderCompletedToGetBuff;
    [SerializeField] private List<float> _timeWaitToNextCustomer;
    [SerializeField] private int GameMoney = 5000;
    [SerializeField] private int GameWinMoney = 50000;
    [SerializeField] private InGameUIBridgeSO _inGameUIBridgeSO;
    public List<CharacterControllerS> CharacterControllerS => _characterControllerS;
    public float TimeWashMultiplier = 1;
    public float TimePrepareMultiplier = 1;
    public float TimeCookMultiplier = 1;
    public float IncomeMultiplier = 1;
    public float OutcomeMultiplier = 1;
    public float TimeToWaittMultiplier = 1;
    public Action<int, int> OnChangeMoney;
    public Action<int, int> onChangeOrder;
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        foreach (var item in _inActiveInteractiveObjects)
        {
            item.gameObject.SetActive(false);
        }
        OnChangeMoney?.Invoke(GameMoney, GameWinMoney);
        onChangeOrder?.Invoke(_currentOrderCompleted, _numberOrderCompletedToGetBuff[_firtPivot]);
    }
    // Start is called before the first frame update
    public void ActiveObject()
    {
        if (_inActiveInteractiveObjects.Count == 0)
            return;
        _inActiveInteractiveObjects[_inActiveInteractiveObjects.Count - 1].gameObject.SetActive(true);
        _inActiveInteractiveObjects.RemoveAt(_inActiveInteractiveObjects.Count - 1);
    }
    public void IncreaseCharacterSpeed(float value)
    {
        foreach (var item in _characterControllerS)
        {
            item.Speed *= (1+value);
        }
    }
    public void ChangeMoney(int value, Vector3 position, float delay = 0)
    {
        SetTextFuncVoke(value, position, delay);
    }

    private async void SetTextFuncVoke(int value, Vector3 position, float delay)
    {
        await UniTask.WaitForSeconds(delay);
        _inGameUIBridgeSO.SetWarningText(value.ToString(), position, true);
        GameMoney += value;
        if (GameMoney < 0)
        {
            GameMoney = 0;
            _inGameUIBridgeSO.SetLose();
        } else if (GameMoney >= GameWinMoney)
        {
            _inGameUIBridgeSO.SetWin();
        }
        OnChangeMoney?.Invoke(GameMoney, GameWinMoney);

    }
    [SerializeField] DeliveryObject _deliveryObject;
    [SerializeField] float _timeToWait = 60;
    public Action<string, float> OnOrderMade;   

    public async void NextOrder()
    {
        float time = UnityEngine.Random.Range(1,3.2f);
        await UniTask.WaitForSeconds(time);
        int index = UnityEngine.Random.Range(1, 270);
        string foodKey = "ufood_des_" + index.ToString();
        OnOrderMade?.Invoke(foodKey, _timeToWait);
    }
    public Action<float> OrderCompleteAction;
    private int _firtPivot = 0;
    private int _currentOrderCompleted = 0;
    public void OrderCompleted(float score = 0)
    {
        if (_firtPivot >= _numberOrderCompletedToGetBuff.Count - 1)
            return;
        _currentOrderCompleted++;
        if (_currentOrderCompleted >=  _numberOrderCompletedToGetBuff[_firtPivot])
        {
            _currentOrderCompleted = 0;
            _firtPivot++;
            _inGameUIBridgeSO.OpenCardPanel();
        }
        OrderCompleteAction?.Invoke(score);
    }
    public float GetWaitTime => _timeWaitToNextCustomer[_firtPivot];

}
