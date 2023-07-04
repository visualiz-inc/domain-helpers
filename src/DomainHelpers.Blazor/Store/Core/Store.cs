namespace DomainHelpers.Blazor.Store.Core;


public class Store<TState, TCommand> : AbstractStore<TState, Command.StateHasChanged<TCommand>>
    where TState : class
    where TCommand : notnull {

    public Store(StateInitializer<TState> initializer) : base(initializer, Reducer) {
    }

    static TState Reducer(TState state, Command.StateHasChanged<TCommand> command) {
        return (TState)command.State;
    }

    public void Mutate(Func<TState, TState> reducer, TCommand? command = default) {
        var state = State;
        var type = GetType();
        ComputedAndApplyState(state, new Command.StateHasChanged<TCommand>(reducer(state), command, type));
    }

    public void Mutate(TState state, TCommand? command = default) {
        ComputedAndApplyState(State, new Command.StateHasChanged<TCommand>(state, command, GetType()));
    }
}


public class Store<TState> : AbstractStore<TState, Command.StateHasChanged>
        where TState : class {

    public Store(StateInitializer<TState> initializer) : base(initializer, Reducer) {
    }

    static TState Reducer(TState state, Command.StateHasChanged command) {
        return (TState)command.State;
    }

    public void Mutate(Func<TState, TState> reducer, Command? command = null) {
        var state = State;
        var type = GetType();
        ComputedAndApplyState(state, new Command.StateHasChanged(reducer(state), command, type));
    }

    public void Mutate(TState state, Command? command = null) {
        ComputedAndApplyState(State, new Command.StateHasChanged(state, command, GetType()));
    }
}
