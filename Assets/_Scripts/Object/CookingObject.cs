using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;
using UI;
using Manager;

using Cysharp.Threading.Tasks;

namespace ObjectS
{
    public class CookingObject : InteractiveObject
    {
        [SerializeField] private ListTabConfigSO _listTabConfigSO;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private GameObject _noi;
        [SerializeField] private AudioClip _cookDone;
        [SerializeField] private AudioClip _takeFood;
        [SerializeField] private IntergridientUI _intergridientUIPRefab;
        private GameController _gameController;
        private CookerState _cookerState = CookerState.Free;
        private WellFoodSO _wellFoodSO;
        private List<BaseFoodSO> _listFoodSO = new List<BaseFoodSO>();
        private IntergridientUI _intergridientUI;
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS == null)
            {
                return false;
            }
            _gameController = gameController;
            if (_cookerState == CookerState.Waiting)
            {
                if (controllerS.CurrentHoldFood == null) 
                {
                    _inGameUIBridgeSO.SetWarningText("mess_notsuitable", transform.position);
                    return false;
                }
                if (_listFoodSO.Contains(controllerS.CurrentHoldFood) == false)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_notsuitable", transform.position);
                    return false;
                }
                var materialSO = controllerS.CurrentHoldFood as MaterialSO;
                if (materialSO == null)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_food", transform.position);
                    return false;
                }
                if (materialSO.NeedToWash)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_wash", transform.position);
                    return false;
                }
            }
            if (_cookerState == CookerState.Cooking)
            {
                _inGameUIBridgeSO.SetWarningText("mess_cooking", transform.position);
                return false;
            }
            if (_cookerState == CookerState.Done)
            {
                if (controllerS.CurrentHoldFood != null)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_carrying", transform.position);
                    return false;
                }
            }
            return base.Interact(controllerS, gameController);
        }
        private void Awake() {
            _particleSystem.Stop();
            _noi.SetActive(false);
        }
        public override void PerformInteraction(GameController gameController)
        {
            _currentController.ChangeState(_currentController.IdleState);
            if (_currentController == null)
                return;
            if (gameController.IsFocused)
            {
                _currentController.RemoveCurrentTarget();
                return;
            }
            if (_cookerState == CookerState.Free)
            {
                gameController.RequireFocussing(_currentController);
                _inGameUIBridgeSO.UseTabGroup(_listTabConfigSO, this);
                return;
            }
            if (_cookerState == CookerState.Waiting)
            {
                if (_currentController.CurrentHoldFood == null)
                    return;
                var index = _listFoodSO.IndexOf(_currentController.CurrentHoldFood);
                if (index == -1)
                    return;
                Debug.Log("Remove " + _currentController.CurrentHoldFood.name);
                _listFoodSO.RemoveAt(index);
                _intergridientUI.RemoveItemAt(index);
                var foodObject = _currentController.CurrentHoldFood.IngameFoodInstance;
                Destroy(foodObject.FoodSO);
                Destroy(foodObject.gameObject);
                _currentController.RemoveHoldFood();
                if (_listFoodSO.Count == 0)
                {
                    _cookerState = CookerState.Cooking;
                    _particleSystem.Play();
                    _available = false;
                    Destroy(_intergridientUI.gameObject);
                    ProcessingFood(_wellFoodSO);
                }
                return;
            }
            if (_cookerState == CookerState.Cooking)
            {
                _inGameUIBridgeSO.SetWarningText("mess_cooking", transform.position);
                return;
            }
            if (_cookerState == CookerState.Done)
            {
                _currentController.SetHoldFood(_wellFoodSO);
                _noi.SetActive(false);
                _cookerState = CookerState.Free;
                _available = true;
                _soundManagerSO.Play(_takeFood);
                return;
            }

        }
        public override void OnFoodSelected(BaseFoodSO food)
        {
            if (_cookerState != CookerState.Free)
                return;
            if (food != null && _currentController != null)
            {
                _wellFoodSO = food as WellFoodSO;
                _cookerState = CookerState.Waiting;
                _listFoodSO = new List<BaseFoodSO>(_wellFoodSO.Intergients);
                
                _intergridientUI = Instantiate(_intergridientUIPRefab, _inGameUIBridgeSO.transform);
                _intergridientUI.SetRecipe(_listFoodSO, transform.position);
                _noi.SetActive(true);
            }
            _gameController.UnFocus();
        }
        public async void ProcessingFood(BaseFoodSO food)
        {
            bool isNew;
            var source = _soundManagerSO.RentAudioSource(out isNew);
            source.clip = _onPerformClip;
            source.loop = true;
            source.Play();
            _inGameUIBridgeSO.SetCounter(food.TimeToPrepare, transform.position+ Vector3.up*3);
            await UniTask.WaitForSeconds(food.TimeToPrepare);
            source.Stop();
            if (isNew)
                Destroy(source.gameObject);
            else
                _soundManagerSO.ReleaseAudioSource(source);
            _available = true;
            _particleSystem.Stop();
            _soundManagerSO.Play(_cookDone);
            _cookerState = CookerState.Done;
        }
    }
    public enum CookerState
    {
        Free,
        Waiting,
        Cooking,
        Done
    }
}

