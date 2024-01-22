namespace DomainHelpers.Blazor.Store.Core;

public delegate object NextStoreMiddlewareCallback(object? state, object? command);

public delegate RootState NextProviderMiddlewareCallback(RootState? state, IStateChangedEventArgs<object, object> e);

public abstract class MiddlewareHandler : IDisposable {
    /// <summary>
    ///  Called on the store initialized.
    /// </summary>
    /// <param name="provider">The StoreProvider.</param>
    internal protected virtual async Task InitializedAsync() {
        await OnInitializedAsync();
    }

    protected virtual Task OnInitializedAsync() {
        return Task.CompletedTask;
    }

    public virtual RootState? HandleProviderDispatch(
        RootState? state,
        IStateChangedEventArgs<object, object> e,
        NextProviderMiddlewareCallback next
    ) => next(state, e);

    public virtual object? HandleStoreDispatch(
        object? state,
        object? command,
        NextStoreMiddlewareCallback next
    ) => next(state, command);

    public virtual void Dispose() {

    }
}