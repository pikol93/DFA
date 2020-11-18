using System;
using System.Collections.Generic;

namespace Pikol93.DFA
{
    public abstract class DFAState<TSignal, TState> : IDisposable
        where TSignal : Enum
        where TState : Enum
    {
        private Dictionary<TSignal, DFATransition<TState>> _transitions = 
            new Dictionary<TSignal, DFATransition<TState>>();
        public Dictionary<TSignal, DFATransition<TState>> Transitions
        {
            get => _transitions;
            set => AddTransitions(value);
        }

        #region IDisposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Transitions.Clear();
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

        public void AddTransitions(IEnumerable<KeyValuePair<TSignal, DFATransition<TState>>> transitions)
        {
            foreach (KeyValuePair<TSignal, DFATransition<TState>> transition in transitions)
            {
                Transitions.Add(transition.Key, transition.Value);
            }
        }

        internal DFATransition<TState> GetTransition(TSignal signal)
        {
            try
            {
                return Transitions[signal];
            }
            catch (KeyNotFoundException) { /* Transition not found */ }

            return null;
        }

        public virtual void EnterState(object[] args) { }
        public virtual void ExitState() { }
    }
}