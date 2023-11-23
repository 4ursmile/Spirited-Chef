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
    [SerializeField] private AudioClip _coinClip;
    [SerializeField] private SoundManagerSO _soundManagerSO;
    [SerializeField] private AudioClip _orderClip;
    public List<CharacterControllerS> CharacterControllerS => _characterControllerS;
    public float TimeWashMultiplier = 1;
    public float TimePrepareMultiplier = 1;
    public float TimeCookMultiplier = 1;
    public float IncomeMultiplier = 1;
    public float OutcomeMultiplier = 1;
    public float TimeToWaittMultiplier = 1;
    public float IncomeReceiveMultiplier = 1;
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
    public void ActiveWaitingObject()
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
    // Start is called before the first frame update
    public void ActiveObject()
    {
        if (_inActiveInteractiveObjects.Count == 0)
            return;
        _inActiveInteractiveObjects[_inActiveInteractiveObjects.Count - 1].gameObject.SetActive(true);
        _inActiveInteractiveObjects.RemoveAt(_inActiveInteractiveObjects.Count - 1);
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
        _soundManagerSO.Play(_coinClip);
        OnChangeMoney?.Invoke(GameMoney, GameWinMoney);

    }
    [SerializeField] DeliveryObject _deliveryObject;
    [SerializeField] float _timeToWait = 60;
    public Action<string, float> OnOrderMade;   

    public async void NextOrder()
    {
        float time = UnityEngine.Random.Range(1,2.2f);
        await UniTask.WaitForSeconds(time);
        _soundManagerSO.Play(_orderClip);
        int index = UnityEngine.Random.Range(1, 270);
        string foodKey = "ufood_des_" + index.ToString();
        OnOrderMade?.Invoke(foodKey, _timeToWait*TimeToWaittMultiplier);
    }
    public Action<float> OrderCompletedAction;
    private int _firtPivot = 0;
    private int _currentOrderCompleted = 0;
    [Header("Score parameter")]
    [SerializeField] private float _scoreGood = 0.65f;
    [SerializeField] private float _scoreNormal = 0.62f;
    [SerializeField] private List<string> _reactionWithScoreKey;
    public string GetReactionKey(float score)
    {
        if (score == 0)
            return _reactionWithScoreKey[0];
        if (score < _scoreNormal)
            return _reactionWithScoreKey[1];
        if (score < _scoreGood)
            return _reactionWithScoreKey[2];
        return _reactionWithScoreKey[3];
    }
    public float GetPricePercent(float score)
    {
        if (score == 0)
            return 0.0f;
        if (score < _scoreNormal)
            return 0.5f;
        if (score < _scoreGood)
            return 1f;
        return 1.5f;
    }
    public float GetTipsPercent(float score)
    {
        float tips = score - _scoreGood;
        if (tips < 0)
            return 0;
        return tips+0.3f;
    }
    [SerializeField] private List<AudioClip> _scoreClips;
    public void OrderComplete(float score = 0)
    {
        OrderCompletedAction?.Invoke(score);
        if (score == 0) 
        {
            _soundManagerSO.Play(_scoreClips[0]);
            return;
        }
        if (score < _scoreNormal)
        {
            _soundManagerSO.Play(_scoreClips[1]);
            return;
        }
        if (score < _scoreGood)
        {
            _soundManagerSO.Play(_scoreClips[2]);
        } else
        {
            _soundManagerSO.Play(_scoreClips[3]);
        }
        if (_firtPivot >= _numberOrderCompletedToGetBuff.Count - 1)
            return;
        _currentOrderCompleted++;
        if (_currentOrderCompleted >=  _numberOrderCompletedToGetBuff[_firtPivot])
        {
            _currentOrderCompleted = 0;
            _firtPivot++;
            _inGameUIBridgeSO.OpenCardPanel();
        }
    }
    public float GetWaitTime => _timeWaitToNextCustomer[_firtPivot];

}
