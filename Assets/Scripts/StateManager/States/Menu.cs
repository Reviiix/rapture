using System;
using UnityEngine;

public class Menu : State
{
    public override void OnStateEnter(Action callBack)
    {
        Debug.Log($"Entering {nameof(Menu)} state.");
        callBack();
    }
}
