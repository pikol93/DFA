using System;

namespace Pikol93.DFA
{
    public class DFATransition<TState> where TState : Enum
    {
        public TState TargetState { get; }
        public Func<object[]> GetArguments { get; }

        public DFATransition(TState targetState, Func<object[]> getArgumentsDelegate)
        {
            TargetState = targetState;
            GetArguments = getArgumentsDelegate;
        }
    }
}