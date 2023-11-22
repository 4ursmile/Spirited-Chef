
using Manager;
using UnityEngine;
using Behaviour;
using DG.Tweening;
using Architecture;

namespace ObjectS
{
    public class StepObject : InteractiveObject
    {

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private bool _isOpened = true;
        private BaseFoodSO _backwardFood;
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {

            if (controllerS == null)
            {
                return false;
            }
            if (_isOpened)
            {
                if (controllerS.CurrentHoldFood == null) 
                {
                    _inGameUIBridgeSO.SetWarningText("mess_available", transform.position);
                    return false;
                } 
            }  else
            {
                if (controllerS.CurrentHoldFood != null)
                {
                    _inGameUIBridgeSO.SetWarningText("mess_carrying", transform.position);
                    return false;
                }

            }  
            
            return base.Interact(controllerS, gameController);
        }
        public override void PerformInteraction(GameController gameController)
        {

            _currentController?.ChangeState(_currentController.IdleState);
            if (_currentController == null)
                return;
            if (_isOpened)
            {
                if (_currentController.CurrentHoldFood != null)
                {
                    var food = _currentController.CurrentHoldFood;
                    Destroy(food.IngameFoodInstance.gameObject);
                    _currentController.RemoveHoldFood();
                    _backwardFood = food;
                    _spriteRenderer.gameObject.SetActive(true);
                    _spriteRenderer.sprite = food.Icon;
                    _isOpened = false;
                    return;
                }
            }  else
            {
                    _currentController.SetHoldFood(_backwardFood);
                    _spriteRenderer.gameObject.SetActive(false);
                    _backwardFood = null;
                    _isOpened = true;
                    return;
            }

        }

    }
}


