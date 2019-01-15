using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.FSM
{
    public class StateMachine<T>
    {
        private T owner;

        public State<T> CurrentState { get; set; }
        public State<T> PreviousState { get; set; }
        public State<T> GlobalState { get; set; }

        public StateMachine(T owner)
        {
            this.owner = owner;
        }

        public void Update()
        {
            if (GlobalState != null) GlobalState.Execute(owner);
            if (CurrentState != null) CurrentState.Execute(owner);
        }

        public void ChangeState(State<T> newState)
        {
            PreviousState = CurrentState;
            CurrentState.Exit(owner);
            CurrentState = newState;
            CurrentState.Enter(owner);

        }

        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }

        public bool IsInState(State<T> state)
        {
            return CurrentState == state;
        }

        public void SetCurrentState(State<T> state)
        {
            CurrentState = state;
        }

        public void SetPreviousState(State<T> state)
        {
            PreviousState = state;
        }

        public void SetGlobalState(State<T> state)
        {
            GlobalState = state;
        }

        public bool HandleMessage(Telegram message)
        {
            if (CurrentState != null && CurrentState.OnMessage(owner, message))
                return true;

            if (GlobalState != null && GlobalState.OnMessage(owner, message))
                return true;

            return false;
        }

        
    }
}