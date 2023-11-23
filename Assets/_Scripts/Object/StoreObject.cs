using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Manager;
using ObjectS;
using UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
namespace ObjectS
{
    public class StoreObject : InteractiveObject
    {
        [SerializeField] private ListTabConfigSO _listTabConfigSO;
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS == null)
            {
                return false;
            }
            if (controllerS.CurrentHoldFood != null) 
            {
                _inGameUIBridgeSO.SetWarningText("mess_carrying", transform.position);
                return false;
            }
            return base.Interact(controllerS, gameController);
        }
        public override void PerformInteraction(CharacterControllerS controller, GameController gameController)
        {
            if (gameController.IsFocused)
            {
                controller.RemoveCurrentTarget();
                return;
            }
            base.PerformInteraction(controller, gameController);
            _available = false;
            _gameController.RequireFocussing(_currentController);
            _inGameUIBridgeSO.UseTabGroup(_listTabConfigSO, this);
        }
        public override void OnFoodSelected(BaseFoodSO food)
        {
            if (food != null && _currentController != null)
            {
                _currentController.SetHoldFood(food);
                int price = (int)(food.Price*UniversalObjectInstance.Instance.OutcomeMultiplier);
                UniversalObjectInstance.Instance.ChangeMoney(-price, _currentController.transform.position, 1f);
                _currentController.RemoveCurrentTarget();
            }
            _gameController.UnFocus();
            _currentController = null;
            _available = true;
        }
    }
}

