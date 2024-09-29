using System;

namespace StateManager.States.Base
{
    public interface IState
    {
        public void OnStateEnter(Action callBack);
    }
}
