namespace DomainHelpers.Blazor.Store.Blazor;
internal class StoreObserver(Action<IStateChangedEventArgs<object, object>> action)
        : IObserver<IStateChangedEventArgs<object, object>> {
    readonly Action<IStateChangedEventArgs<object, object>> _action = action;

    public void OnCompleted() {
        throw new NotSupportedException();
    }

    public void OnError(Exception error) {
        throw new NotSupportedException();
    }

    public void OnNext(IStateChangedEventArgs<object, object> value) {
        _action(value);
    }
}