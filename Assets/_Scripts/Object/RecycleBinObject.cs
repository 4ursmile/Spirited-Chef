
using Manager;
using UnityEngine;
using Behaviour;
using DG.Tweening;
using Architecture;

namespace ObjectS
{
    public class RecycleBinObject : InteractiveObject
    {
        [SerializeField] private GameObject _door;
        [SerializeField] private float _throwDuration = 0.7f;
        [SerializeField] private Ease _throwEase = Ease.OutBack;
        [SerializeField] private Vector3 _throwOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] private float _throwHeight = 3f;
        [SerializeField] private float _doorDelay = 0.3f;
        [SerializeField] private float _doorDuration = 0.5f;
        [SerializeField] private float _doorAngle = 90f;

        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {

            if (controllerS == null)
            {
                return false;
            }
            if (controllerS.CurrentHoldFood == null) 
            {
                _inGameUIBridgeSO.SetWarningText("mess_available", transform.position);
                return false;
            }
            return base.Interact(controllerS, gameController);
        }
        public override void PerformInteraction(CharacterControllerS controllerS, GameController gameController)
        {
            base.PerformInteraction(controllerS, gameController);
            if (_currentController.CurrentHoldFood != null)
            {
                var foodObject = _currentController.CurrentHoldFood;
                foodObject.UnSetTarget();
                _currentController.RemoveHoldFood();
                _currentController.RemoveCurrentTarget();
                var food = foodObject.FoodSO;
                var seq = DOTween.Sequence();
                seq.Append(
                    foodObject.transform.DOJump(transform.position + _throwOffset, _throwHeight, 1, _throwDuration).SetEase(_throwEase)
                );
                seq.Insert(_doorDelay, _door.transform.DOLocalRotate(new Vector3(0, _doorAngle, 0), _doorDuration/2).SetEase(_throwEase));
                seq.Insert(_doorDelay + _doorDuration/2, _door.transform.DOLocalRotate(new Vector3(0, 0,0), _doorDuration/2).SetEase(_throwEase));
                seq.Play().OnComplete(() => {
                    int price = (int)((food.Price/5)*UniversalObjectInstance.Instance.IncomeReceiveMultiplier);
                    UniversalObjectInstance.Instance.ChangeMoney(price, _currentController.transform.position, 0.2f);
                    foodObject.Destroy();
                });
                    

                _soundManagerSO.Play(_onPerformClip);
                return;
                
            }
        }

    }
}

