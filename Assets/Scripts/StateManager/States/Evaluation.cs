using System;
using StateManager.States.Base;
using UnityEngine;

namespace StateManager.States
{
    public class Evaluation : State
    {
        public static Action<bool> OnEvaluationComplete;
    
        public override void OnStateEnter(Action callBack)
        {
            Debug.Log($"Entering {nameof(Evaluation)} state.");
            OnEvaluationComplete?.Invoke(Evaluator.Instance.IsMatch());
            callBack();
        }
    }
}
