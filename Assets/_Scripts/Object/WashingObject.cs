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
    public class WashingObject : InteractiveObject
    {
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
            if (!controllerS.CurrentHoldFood.NeedToWash)
            {
                _inGameUIBridgeSO.SetWarningText("mess_nonneed", transform.position);
                return false;
            }
            return base.Interact(controllerS, gameController);
        }
        private BaseFoodSO _backwardFood;
        public override void PerformInteraction(CharacterControllerS controllerS, GameController gameController)
        {
            base.PerformInteraction(controllerS, gameController);
            var food = _currentController.CurrentHoldFood.FoodSO as MaterialSO;
            if (food == null)
                return;
            _available = false;
            _currentController.StarWorking();
            _inGameUIBridgeSO.SetCounter(food.WashTime, transform.position);
            ProcessingFood(food);
        }

        public async void ProcessingFood(MaterialSO food)
        {
            _soundManagerSO.RentAudioSource(_onPerformClip, food.WashTime);
            _inGameUIBridgeSO.SetPartical(0, transform.position, food.WashTime);
            await UniTask.WaitForSeconds(food.WashTime*UniversalObjectInstance.Instance.TimePrepareMultiplier);
            _currentController.EndWorking();
            _currentController.CurrentHoldFood.NeedToWash = false;
            _available = true;
        }
 
    }
}
