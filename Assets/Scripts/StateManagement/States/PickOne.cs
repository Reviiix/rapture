using System;
using StateManagement.States.Base;
using UnityEngine;

namespace StateManagement.States
{
    public class PickOne : State
    {
        public override void OnStateEnter(Action callBack)
        {
            Debug.Log($"Entering {nameof(PickOne)} state.");
            callBack();
        }
    }
}
