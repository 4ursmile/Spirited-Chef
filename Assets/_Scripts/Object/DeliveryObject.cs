
using Manager;
using UnityEngine;
using Behaviour;
using DG.Tweening;
using Architecture;

namespace ObjectS
{
    public class DeliveryObject : InteractiveObject
    {

        [SerializeField] private float _throwDuration = 0.7f;
        [SerializeField] private Ease _throwEase = Ease.OutBack;
        [SerializeField] private Vector3 _throwOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] private float _throwHeight = 3f;
        [SerializeField] private InGameUIBridgeSO _uiBridge;
        private bool _isOrderd = false;
        private string _currentOrder;
        public bool IsOrderd => _isOrderd;
        public void MakerOrder(string order, float waitingTime)
        {
            _currentOrder = order;
            _isOrderd = true;
        }
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {
            if (_isOrderd == false)
            {
                _inGameUIBridgeSO.SetWarningText("mess_available", transform.position);
                return false;
            }
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
                
                seq.Play().onComplete += () => {
                    float Score = Database.Score(_currentOrder, food.EmbeddedKey);
                    UniversalObjectInstance.Instance.ChangeMoney((int)(Score*food.Price), _currentController.transform.position, 0.2f);
                    Destroy(foodObject.FoodSO);
                    Destroy(foodObject.gameObject);
                    _currentController.RemoveHoldFood();
                    _currentController.RemoveCurrentTarget();
                    UniversalObjectInstance.Instance.OrderCompleted(Score);
                };
                _soundManagerSO.Play(_onPerformClip);
                return;
            }

        }
        public void GetScore()
        {
        }
        public override void Init()
        {
            base.Init();
            UniversalObjectInstance.Instance.OnOrderMade += MakerOrder;
        }
        private void OnDestroy() {
            UniversalObjectInstance.Instance.OnOrderMade -= MakerOrder;
        }
    }
}

