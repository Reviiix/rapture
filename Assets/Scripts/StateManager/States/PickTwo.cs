using System;
using StateManager.States.Base;
using UnityEngine;

namespace StateManager.States
{
    public class PickTwo : State
    {
        public override void OnStateEnter(Action callBack)
        {
            Debug.Log($"Entering {nameof(PickOne)} state.");
            callBack();
        }
    }
}