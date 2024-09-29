using System;
using UnityEngine;

public class PickTwo : State
{
    public override void OnStateEnter(Action callBack)
    {
        Debug.Log($"Entering {nameof(PickOne)} state.");
        callBack();
    }
}