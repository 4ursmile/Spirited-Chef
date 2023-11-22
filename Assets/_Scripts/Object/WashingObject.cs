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
            MaterialSO materialSO = controllerS.CurrentHoldFood as MaterialSO;
            if (materialSO == null)
            {
                _inGameUIBridgeSO.SetWarningText("mess_food", transform.position);
                return false;
            } 
            if (!materialSO.NeedToWash)
            {
                _inGameUIBridgeSO.SetWarningText("mess_nonneed", transform.position);
                return false;
            }
            return base.Interact(controllerS, gameController);
        }
        private BaseFoodSO _backwardFood;
        public override void PerformInteraction(GameController gameController)
        {
            _currentController.ChangeState(_currentController.IdleState);
            if (_currentController == null)
                return;
        
            var food = _currentController.CurrentHoldFood as MaterialSO;
            if (food == null)
                return;
            _available = false;
            _currentController.StarWorking();
            _inGameUIBridgeSO.SetCounter(food.WashTime, transform.position);
            ProcessingFood(food);
        }

        public async void ProcessingFood(MaterialSO food)
        {
            bool isNew;
            var source = _soundManagerSO.RentAudioSource(out isNew);
            source.clip = _onPerformClip;
            source.loop = true;
            source.Play();
            _inGameUIBridgeSO.SetPartical(0, transform.position, food.WashTime);
            await UniTask.WaitForSeconds(food.WashTime);
            source.Stop();
            if (isNew)
                Destroy(source.gameObject);
            else
                _soundManagerSO.ReleaseAudioSource(source);
            _currentController.EndWorking();
            _currentController.CurrentHoldFood.NeedToWash = false;
            _available = true;
        }
 
    }
}
