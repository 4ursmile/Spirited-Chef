
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
            if (controllerS.CurrentHoldFood == null && controllerS.CurrentHoldObject == null) 
            {
                _inGameUIBridgeSO.SetWarningText("mess_available", transform.position);
                return false;
            }
            return base.Interact(controllerS, gameController);
        }
        public override void PerformInteraction(GameController gameController)
        {

            _currentController?.ChangeState(_currentController.IdleState);
            if (_requireFocus && gameController.IsFocused)
                return;
            if (_currentController == null)
                return;
            if (_currentController.CurrentHoldFood != null)
            {
                var food = _currentController.CurrentHoldFood;
                var foodObject = food.IngameFoodInstance;
                foodObject.UnSetTarget();
                var seq = DOTween.Sequence();
                seq.Append(
                    foodObject.transform.DOJump(transform.position + _throwOffset, _throwHeight, 1, _throwDuration).SetEase(_throwEase)
                );
                seq.Insert(_doorDelay, _door.transform.DOLocalRotate(new Vector3(0, _doorAngle, 0), _doorDuration/2).SetEase(_throwEase));
                seq.Insert(_doorDelay + _doorDuration/2, _door.transform.DOLocalRotate(new Vector3(0, 0,0), _doorDuration/2).SetEase(_throwEase));
                seq.Play().onComplete += () => {
                    UniversalObjectInstance.Instance.ChangeMoney(food.Price/5, _currentController.transform.position, 0.2f);
                    Destroy(foodObject.FoodSO);
                    Destroy(foodObject.gameObject);
                    _currentController.RemoveHoldFood();
                    _currentController.RemoveCurrentTarget();
                };
                _soundManagerSO.Play(_onPerformClip);
                return;
                
            }
             if (_currentController.CurrentHoldObject != null)
            {
                var foodObject = _currentController.CurrentHoldObject;
                _currentController.RemoveHoldSomething();

                var seq = DOTween.Sequence();
                seq.Append(
                    foodObject.transform.DOJump(transform.position + _throwOffset, _throwHeight, 1, _throwDuration).SetEase(_throwEase)
                );
                seq.Insert(_doorDelay, _door.transform.DOLocalRotate(new Vector3(0, _doorAngle, 0), _doorDuration/2).SetEase(_throwEase));
                seq.Insert(_doorDelay + _doorDuration/2, _door.transform.DOLocalRotate(new Vector3(0, 0,0), _doorDuration/2).SetEase(_throwEase));
                seq.Play().onComplete += () => {
                    Destroy(foodObject.gameObject);
                    UniversalObjectInstance.Instance.ChangeMoney(10, _currentController.transform.position, 0.2f);
                    _currentController.RemoveCurrentTarget();
                };
                return;
            }

        }

    }
}

