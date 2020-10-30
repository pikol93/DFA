using System;
using System.Collections.Generic;

namespace Pikol93.DFA
{
    public abstract class DFATree<TSignal, TState> : IDisposable
        where TSignal : Enum
        where TState : Enum
    {
        protected Dictionary<TState, DFAState<TSignal, TState>> States { get; private set; }
        protected TState CurrentState { get; private set; }

        #region IDisposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (IDisposable obj in States.Values)
                    {
                        obj.Dispose();
                    }

                    States.Clear();
                    States = null;
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public void CreateStates(
            Dictionary<TState, DFAState<TSignal, TState>> states,
            TState entryState)
        {
            States = states;
            CurrentState = entryState;
            States[CurrentState].EnterState(null);
        }

        public void InvokeSignal(TSignal signal)
        {
            DFATransition<TState> transition = States[CurrentState].GetTransition(signal);
            
            if (transition != null)
            {
                object[] args = transition.GetArguments?.Invoke();
                ChangeState(transition.TargetState, args);
            }
        }

        private void ChangeState(TState targetState, object[] args)
        {
            if (!States.ContainsKey(targetState))
                return;
            
            States[CurrentState]?.ExitState();
            States[targetState]?.EnterState(args);

            CurrentState = targetState;
        }
    }
}