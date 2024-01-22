namespace DomainHelpers.Blazor.Store.Core;

public delegate TState Reducer<TState,TMessage>(TState state, TMessage? command)
    where TState : class
    where TMessage : notnull;