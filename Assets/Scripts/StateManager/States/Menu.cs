using System;
using StateManager.States.Base;
using UnityEngine;

namespace StateManager.States
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
