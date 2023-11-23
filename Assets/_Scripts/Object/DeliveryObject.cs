
using Manager;
using UnityEngine;
using Behaviour;
using DG.Tweening;
using Architecture;
using System.Collections;

namespace ObjectS
{
    public class DeliveryObject : InteractiveObject
    {

        [SerializeField] private float _throwDuration = 0.7f;
        [SerializeField] private Ease _throwEase = Ease.OutBack;
        [SerializeField] private Vector3 _throwOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] private float _throwHeight = 3f;
        [SerializeField] private InGameUIBridgeSO _uiBridge;
        [SerializeField] private AudioClip _onOrderFailClip;
        private bool _isOrderd = false;
        private string _currentOrder;
        public bool IsOrderd => _isOrderd;
        public void MakerOrder(string order, float waitingTime)
        {
            _currentOrder = order;
            _isOrderd = true;
            _uiBridge.OpenOrderMessageUI(order, waitingTime);
            StartCoroutine(Waiting(waitingTime));
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
        public override void PerformInteraction(CharacterControllerS controllerS, GameController gameController)
        {

            base.PerformInteraction(controllerS, gameController);
            if (_currentController.CurrentHoldFood != null)
            {
                StopAllCoroutines();
                var foodObject = _currentController.CurrentHoldFood;
                var food = foodObject.FoodSO;
                _currentController.RemoveHoldFood();
                _currentController.RemoveCurrentTarget();
                foodObject.UnSetTarget();
                var seq = DOTween.Sequence();
                seq.Append(
                    foodObject.transform.DOJump(transform.position + _throwOffset, _throwHeight, 1, _throwDuration).SetEase(_throwEase)
                );
                
                seq.Play().OnComplete( () => {
                    float Score = Database.Score(_currentOrder, food.EmbeddedKey);
                    int baseMoney = (int)(food.Price*UniversalObjectInstance.Instance.GetPricePercent(score: Score)*UniversalObjectInstance.Instance.IncomeMultiplier);
                    UniversalObjectInstance.Instance.ChangeMoney(baseMoney, _currentController.transform.position, 0.1f);
                    int bonusMoney = (int)(baseMoney * UniversalObjectInstance.Instance.GetTipsPercent(score: Score)*UniversalObjectInstance.Instance.IncomeMultiplier);
                    if (bonusMoney > 0)
                    {
                        UniversalObjectInstance.Instance.ChangeMoney(bonusMoney, _currentController.transform.position, 0.1f);
                    }
                    foodObject.transform.DOKill();
                    foodObject.Destroy();
                    UniversalObjectInstance.Instance.OrderComplete(Score);
                    _inGameUIBridgeSO.CloseOrderMessageUI();
                });
                _soundManagerSO.Play(_onPerformClip);
                return;
            }

        }
        public void GetScore()
        {
        }
        public IEnumerator Waiting(float time)
        {
            yield return new WaitForSeconds(time);
            _inGameUIBridgeSO.CloseOrderMessageUI();
            UniversalObjectInstance.Instance.OrderComplete(0);
            _soundManagerSO.Play(_onOrderFailClip);
            _isOrderd = false;
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

