using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.FSM
{
    public abstract class State<T>
    {
        public abstract void Enter(T owner);
        public abstract void Execute(T owner);
        public abstract void Exit(T owner);
    }

    public abstract class SingletonState<T, T1> : State<T1> where T : new()
    {
        private static T instance = default(T);
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();

                return instance;
            }
        }
    }
}
