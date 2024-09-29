using System;

namespace StateManagement.States.Base
{
    public interface IState
    {
        public void OnStateEnter(Action callBack);
    }
}
