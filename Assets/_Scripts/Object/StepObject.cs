
using Manager;
using UnityEngine;
using Behaviour;
using DG.Tweening;
using Architecture;
using UI;

namespace ObjectS
{
    public class StepObject : InteractiveObject
    {

        [SerializeField] private Vector3 _offset;
        public Vector3 Offset => _offset;
        private InGameFoodInstance _backwardFood;
        public InGameFoodInstance BackwardFood {get => _backwardFood; set => _backwardFood = value;}
        public IBaseObjectState FreeSate {get; private set;}
        public IBaseObjectState BusySate {get; private set;}
        private IntergridientUI _intergridientUI;
        public IntergridientUI IUI {get => _intergridientUI; set => _intergridientUI = value;}
        
        public override bool Interact(CharacterControllerS controllerS, GameController gameController)
        {

            if (controllerS == null)
            {
                return false;
            }
            if (!_stateMachine.Interact(controllerS, gameController))
                return false;
            return base.Interact(controllerS, gameController);
        }
        public override void PerformInteraction(CharacterControllerS controller, GameController gameController)
        {
            base.PerformInteraction(controller, gameController);
            _stateMachine.Perform(controller, gameController);
        }
        public override void Init()
        {
            base.Init();
            FreeSate = new FreeStepState();
            FreeSate.Init(this);
            BusySate = new BusyStepSate();
            BusySate.Init(this);
            _stateMachine = new ObjectStateMachine(FreeSate);
        }

    }
    public class FreeStepState : IBaseObjectState
    {
        StepObject _interactiveObject;
        public StepObject IO => _interactiveObject;
        public void Init(InteractiveObject interactiveObject)
        {
            _interactiveObject = (StepObject)interactiveObject;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        public bool OnInteract(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS.CurrentHoldFood == null) 
            {
                return false;
            }
            return true;
        }

        public void OnPerform(CharacterControllerS controllerS, GameController gameController)
        {
            
            if (IO.CurrentController.CurrentHoldFood != null)
            {
                var foodObject = IO.CurrentController.CurrentHoldFood;
                IO.CurrentController.RemoveHoldFood();
                IO.BackwardFood = foodObject;
                foodObject.SetTargetOnly(IO.transform);
                IO.IUI = IO.UIBridge.GetIntergridientUI();
                IO.IUI.SetRecipe(foodObject.FoodSO, IO.transform.position);
                IO.ChangeState(IO.BusySate);
                return;
            }
        }
    }
    public class BusyStepSate : IBaseObjectState
    {
        StepObject _interactiveObject;
        public StepObject IO => _interactiveObject;
        public void Init(InteractiveObject interactiveObject)
        {
            _interactiveObject = (StepObject)interactiveObject;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        public bool OnInteract(CharacterControllerS controllerS, GameController gameController)
        {
            if (controllerS.CurrentHoldFood != null)
                {
                    _interactiveObject.UIBridge.SetWarningText("mess_carrying", _interactiveObject.transform.position);
                    return false;
                }
            return true;
        }

        public void OnPerform(CharacterControllerS controllerS, GameController gameController)
        {
            IO.CurrentController.SetHoldFood(IO.BackwardFood);
            IO.BackwardFood = null;
            IO.IUI.RemoveItemAt(0);
            IO.UIBridge.ReleaseIntergridientUI(IO.IUI);
            IO.IUI = null;
            IO.ChangeState(IO.FreeSate);
            return;
        }
    }
}


