using System;
using UnityEngine;

namespace StateManager.States.Base
{
    [Serializable]
    public abstract class State : MonoBehaviour, IState
    {
        public bool progressImmediately;
    
        public virtual void OnStateEnter(Action callBack)
        {
            throw new NotImplementedException();
        }
    }
}
