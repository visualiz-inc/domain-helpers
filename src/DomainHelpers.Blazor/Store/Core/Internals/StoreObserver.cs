namespace DomainHelpers.Blazor.Store.Core.Internals;
internal class GeneralObserver<T>(Action<T> action) : IObserver<T> {
    readonly Action<T> _action = action;

    public void OnCompleted() {
        throw new NotImplementedException();
    }

    public void OnError(Exception error) {
        throw new NotImplementedException();
    }

    public void OnNext(T value) {
        _action(value);
    }
}

internal class StoreObserver<TState, TCommand>(Action<IStateChangedEventArgs<TState, TCommand>> action)
    : IObserver<IStateChangedEventArgs<TState, TCommand>>
    where TState : class
    where TCommand : Command {
    readonly Action<IStateChangedEventArgs<TState, TCommand>> _action = action;

    public void OnCompleted() {
        throw new NotImplementedException();
    }

    public void OnError(Exception error) {
        throw new NotImplementedException();
    }

    public void OnNext(IStateChangedEventArgs<TState, TCommand> value) {
        _action(value);
    }
}

internal class StoreProviderObserver(Action<RootStateChangedEventArgs> action) : IObserver<RootStateChangedEventArgs> {
    readonly Action<RootStateChangedEventArgs> _action = action;

    public void OnCompleted() {
        throw new NotImplementedException();
    }

    public void OnError(Exception error) {
        throw new NotImplementedException();
    }

    public void OnNext(RootStateChangedEventArgs value) {
        _action(value);
    }
}