using System;
using System.Collections.Generic;

namespace Pikol93.DFA
{
    public abstract class DFAState<TSignal, TState> : IDisposable 
        where TSignal : Enum
        where TState : Enum
    {
        public Dictionary<TSignal, DFATransition<TState>> transitions;

        private bool _disposed;

        public virtual void EnterState(object[] args) { }
        public virtual void UpdateState(float delta) { }
        public virtual void UpdateRare() { }
        public virtual void ExitState() { }

        internal DFATransition<TState> GetTransition(TSignal signal)
        {
            return transitions[signal];
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    transitions.Clear();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}