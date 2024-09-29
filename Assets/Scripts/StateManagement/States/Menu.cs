using System;
using StateManagement.States.Base;
using UnityEngine;

namespace StateManagement.States
{
    public class Menu : State
    {
        public override void OnStateEnter(Action callBack)
        {
            Debug.Log($"Entering {nameof(Menu)} state.");
            callBack();
        }
    }
}
