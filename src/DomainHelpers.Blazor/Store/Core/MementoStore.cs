using DomainHelpers.Blazor.Store.Core.History;

namespace DomainHelpers.Blazor.Store.Core;
public abstract class MementoStore<TState, TMessage>(
    StateInitializer<TState> initializer,
    HistoryManager historyManager
    )
    : AbstractMementoStore<TState, Command.StateHasChanged<TState, TMessage>>(initializer, historyManager, Reducer)
        where TState : class
        where TMessage : notnull {

    /// <summary>
    /// Reduces the state using the provided StateHasChanged command.
    /// </summary>
    /// <param name="state">The current state.</param>
    /// <param name="message">The StateHasChanged command to apply.</param>
    /// <returns>The new state after applying the command.</returns>
    static TState Reducer(TState state, Command.StateHasChanged<TState, TMessage> message) {
        return message.State;
    }

    /// <summary>
    /// Mutates the state using a reducer function.
    /// </summary>
    /// <param name="reducer">The reducer function to apply.</param>
    /// <param name="message">The message that describes what state change has occurred.</param>
    public void Mutate(Func<TState, TState> reducer, TMessage? message = default) {
        var state = State;
        var type = GetType();
        ComputedAndApplyState(state, new Command.StateHasChanged<TState, TMessage>(reducer(state), message, type));
    }

    /// <summary>
    /// Mutates the state using a new state.
    /// </summary>
    /// <param name="state">The new state to apply.</param>
    /// <param name="message">The message that describes what state change has occurred.</param>
    public void Mutate(TState state, TMessage? command = default) {
        ComputedAndApplyState(State, new Command.StateHasChanged<TState, TMessage>(state, command, GetType()));
    }
}

/// <summary>
/// Represents a store for managing state of type TState.
/// You can observe the state by subscribing to the StateChanged event.
/// </summary>
/// <typeparam name="TState">The type of state managed by the store.</typeparam>
public class MementoStore<TState>(StateInitializer<TState> initializer, HistoryManager historyManager) : MementoStore<TState, string>(initializer, historyManager)
        where TState : class {
}