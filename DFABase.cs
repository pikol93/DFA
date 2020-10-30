using System;
using System.Collections.Generic;

namespace Pikol93.DFA
{
    public abstract class DFABase<TSignal, TState> : IDisposable
        where TSignal : Enum
        where TState : Enum
    {
        private const float TIME_MIN = 0.1f;
        private const float TIME_MAX = 0.4f;

        // TODO: Remove Random dependency
        protected static readonly Random random = new Random();

        protected Dictionary<TState, DFAState<TSignal, TState>> States { get; private set; }
        protected TState CurrentState { get; private set; }

        private float timeLeft;
        private bool _disposed;

        #region IDisposable

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

        // TODO: Move this to game engine implementation
        /// <summary> Called from game engine, passes update to state </summary>
        /// <param name="delta">Time passed since last frame.</param>
        public void Update(float delta)
        {
            timeLeft -= delta;
            if (timeLeft < 0.0)
            {
                timeLeft = TIME_MIN + (TIME_MAX - TIME_MIN) * (float)random.NextDouble();
                UpdateRare();
            }

            States[CurrentState]?.UpdateState(delta);
        }

        public void InvokeSignal(TSignal signal)
        {
            DFATransition<TState> transition = States[CurrentState].GetTransition(signal);
            ChangeState(transition.TargetState, transition.GetArguments?.Invoke());
        }

        private void UpdateRare()
        {
            States[CurrentState]?.UpdateRare();
        }

        private void ChangeState(TState targetState, object[] args)
        {
            States[CurrentState]?.ExitState();
            States[targetState]?.EnterState(args);

            CurrentState = targetState;
        }
    }
}