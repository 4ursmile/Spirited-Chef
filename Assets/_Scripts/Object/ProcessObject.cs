using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;
using UI;
using Manager;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace ObjectS
{
    public class ProcessObject : InteractiveObject
    {
        [SerializeField] private ListTabConfigSO _listTabConfigSO;
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS == null)
            {
                return false;
            }
            if (controllerS.CurrentHoldFood == null) 
            {
                _inGameUIBridgeSO.SetWarningText("mess_notsuitable", transform.position);
                return false;
            }
            MaterialSO materialSO = controllerS.CurrentHoldFood.FoodSO as MaterialSO;
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
            if (materialSO.ForwardsMaterial.Count == 0)
            {
                _inGameUIBridgeSO.SetWarningText("mess_nothing", transform.position);
                return false;
            }
            return base.Interact(controllerS, gameController);
        }
        private BaseFoodSO _backwardFood;
        public override void PerformInteraction(CharacterControllerS controller, GameController gameController)
        {
            if (gameController.IsFocused)
            {
                controller.RemoveCurrentTarget();
                return;
            }
            base.PerformInteraction(controller, gameController);
            
            gameController.RequireFocussing(controller);
            var materialSO = controller.CurrentHoldFood.FoodSO as MaterialSO;
            if (materialSO == null)
                return;
            var forwards = materialSO.ForwardsMaterial;
            _listTabConfigSO.TabItemObjects[0].FoodSOs = forwards;
            _backwardFood = materialSO;
            _inGameUIBridgeSO.UseTabGroup(_listTabConfigSO, this);
        }
        public override void OnFoodSelected(BaseFoodSO food)
        {
            if (food != null && _currentController != null)
            {
                UniversalObjectInstance.Instance.ChangeMoney(-food.Price + _backwardFood.Price, _currentController.transform.position, 1f);
                _currentController.StarWorking();
                _inGameUIBridgeSO.SetCounter(food.TimeToPrepare, transform.position);
                _available = false;
                ProcessingFood(food, _currentController);
            }

            _gameController.UnFocus();
        }
        public async void ProcessingFood(BaseFoodSO food, CharacterControllerS controllerS)
        {
            _soundManagerSO.RentAudioSource(_onPerformClip, food.TimeToPrepare);
            _inGameUIBridgeSO.SetPartical(0, transform.position, food.TimeToPrepare);
            await UniTask.WaitForSeconds(food.TimeToPrepare*UniversalObjectInstance.Instance.TimePrepareMultiplier);
            controllerS.SetHoldFood(food);
            controllerS.EndWorking();
            _available = true;

        }
 
    }
}
