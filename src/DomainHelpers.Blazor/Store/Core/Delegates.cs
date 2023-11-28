namespace DomainHelpers.Blazor.Store.Core;

public delegate TState Reducer<TState, TCommand>(TState state, TCommand command)
    where TState : class
    where TCommand : Command;