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
        private GameController _gameController;
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS == null)
            {
                return false;
            }
            if (controllerS.CurrentHoldFood != null || controllerS.CurrentHoldObject != null) 
            {
                _inGameUIBridgeSO.SetWarningText("mess_carrying", transform.position);
                return false;
            }
            _gameController = gameController;
            return base.Interact(controllerS, gameController);
        }
        public override void PerformInteraction(GameController gameController)
        {
            _currentController.ChangeState(_currentController.IdleState);
            if (gameController.IsFocused)
            {
                _currentController.RemoveCurrentTarget();
                return;
            }
            if (_currentController == null)
                return;
            gameController.RequireFocussing(_currentController);
            _available = false;
            _inGameUIBridgeSO.UseTabGroup(_listTabConfigSO, this);
        }
        public override void OnFoodSelected(BaseFoodSO food)
        {
            if (food != null && _currentController != null)
            {
                _currentController.SetHoldFood(food);
                UniversalObjectInstance.Instance.ChangeMoney(-food.Price, _currentController.transform.position, 1f);

            }
            _currentController?.RemoveCurrentTarget();
            _available = true;
            _gameController.UnFocus();
        }
    }
}

