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
            if (_cookerState == CookerState.Waiting)
            {
                if (controllerS.CurrentHoldFood == null) 
                {
                    _inGameUIBridgeSO.SetWarningText("mess_notsuitable", transform.position);
                    return false;
                }
                if (_listFoodSO.Contains(controllerS.CurrentHoldFood.FoodSO) == false)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_notsuitable", transform.position);
                    return false;
                }
                var materialSO = controllerS.CurrentHoldFood.FoodSO as MaterialSO;
                if (materialSO == null)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_food", transform.position);
                    return false;
                }
                if (controllerS.CurrentHoldFood.NeedToWash)
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
        public override void PerformInteraction(CharacterControllerS controller, GameController gameController)
        {
            if (gameController.IsFocused)
            {
                controller.RemoveCurrentTarget();
                return;
            }
            base.PerformInteraction(controller, gameController);
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
                var index = _listFoodSO.IndexOf(_currentController.CurrentHoldFood.FoodSO);
                if (index == -1)
                    return;
                _listFoodSO.RemoveAt(index);
                _intergridientUI.RemoveItemAt(index);
                var foodObject = _currentController.CurrentHoldFood;
                foodObject.UnSetTarget();
                foodObject.Destroy();
                _currentController.RemoveHoldFood();
                if (_listFoodSO.Count == 0)
                {
                    _cookerState = CookerState.Cooking;
                    _particleSystem.Play();
                    _available = false;
                    _inGameUIBridgeSO.ReleaseIntergridientUI(_intergridientUI);
                    _intergridientUI = null;
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
                if (_intergridientUI != null)
                {
                    _intergridientUI.RemoveItemAt(0);
                    _inGameUIBridgeSO.ReleaseIntergridientUI(_intergridientUI);
                    _intergridientUI = null;
                }
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
                _intergridientUI = _inGameUIBridgeSO.GetIntergridientUI();
                _intergridientUI.SetRecipe(_listFoodSO, transform.position);
                _noi.SetActive(true);
            }
            _gameController.UnFocus();
        }
        public async void ProcessingFood(BaseFoodSO food)
        {
            _soundManagerSO.RentAudioSource(_onPerformClip, food.TimeToPrepare);

            _inGameUIBridgeSO.SetCounter(food.TimeToPrepare, transform.position+ Vector3.up*3);
            await UniTask.WaitForSeconds(food.TimeToPrepare*UniversalObjectInstance.Instance.TimePrepareMultiplier);
            _available = true;
            _particleSystem.Stop();
            _soundManagerSO.Play(_cookDone);
            _intergridientUI = _inGameUIBridgeSO.GetIntergridientUI();
            _intergridientUI.SetRecipe(_wellFoodSO, transform.position);
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

