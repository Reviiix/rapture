using System;
using UnityEngine;

public class PickOne : State
{
    public override void OnStateEnter(Action callBack)
    {
        Debug.Log($"Entering {nameof(PickOne)} state.");
        callBack();
    }
}
