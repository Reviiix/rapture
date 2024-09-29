using System;
using StateManagement.States.Base;
using UnityEngine;

namespace StateManagement.States
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
