using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.FSM
{
    public class StateMachine<T>
    {
        private T owner;
        private State<T> currentState;
        private State<T> previousState;
        private State<T> globalState;

        public State<T> CurrentState { get { return currentState; } set { currentState = value; } }
        public State<T> PreviousState { get { return previousState; } set { previousState = value; } }
        public State<T> GlobalState { get { return globalState; } set { globalState = value; } }

        public StateMachine(T owner)
        {
            this.owner = owner;
        }

        public void Update()
        {
            currentState.Execute(owner);
        }

        public void ChangeState(State<T> newState)
        {
            previousState = currentState;
            currentState.Exit(owner);
            currentState = newState;
            currentState.Enter(owner);

        }

        public void RevertToPreviousState()
        {
            ChangeState(previousState);
        }

        public bool IsInState(State<T> state)
        {
            return currentState == state;
        }

        public void SetCurrentState(State<T> state)
        {
            currentState = state;
        }

        public void SetPreviousState(State<T> state)
        {
            previousState = state;
        }

        public void SetGlobalState(State<T> state)
        {
            globalState = state;
        }

        
    }
}