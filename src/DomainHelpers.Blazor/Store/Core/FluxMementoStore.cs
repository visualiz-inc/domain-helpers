using DomainHelpers.Blazor.Store.Core.History;

namespace DomainHelpers.Blazor.Store.Core;
public abstract class FluxMementoStore<TState, TMessage>
    : AbstractMementoStore<TState, TMessage>
        where TState : class
        where TMessage : class {
    /// <summary>
    /// Initializes a new instance of the FluxStore class.
    /// </summary>
    /// <param name="initializer">The state initializer for creating the initial state.</param>
    /// <param name="historyManager">The history manager.</param>
    /// <param name="reducer">The reducer function for applying commands to the state.</param>
    protected FluxMementoStore(
        StateInitializer<TState> initializer,
        HistoryManager historyManager,
        Reducer<TState, TMessage> reducer
    ) : base(initializer, historyManager, reducer) {
    }

    /// <summary>
    /// Dispatches a command to the store, which updates the state accordingly.
    /// </summary>
    /// <param name="command">The command to dispatch.</param>
    public void Dispatch(TMessage command) {
        ComputedAndApplyState(State, command);
    }

    /// <summary>
    /// Dispatches a command to the store using a message loader function, which updates the state accordingly.
    /// </summary>
    /// <param name="messageLoader">The function to generate a command based on the current state.</param>
    public void Dispatch(Func<TState, TMessage> messageLoader) {
        ComputedAndApplyState(State, messageLoader(State));
    }
}