using DomainHelpers.Commons.Primitives;
using System.Collections.Concurrent;

namespace DomainHelpers.Blazor.Store.Core;

public interface IStateObservable<out TMessage> :IObservable<IStateChangedEventArgs<TMessage>>
    where TMessage : notnull {
    /// <summary>
    /// Notifies observers that the state of the store has changed.
    /// </summary>
    void StateHasChanged();
}

public abstract class StateObservable<TMessage> : IStateObservable<TMessage>
    where TMessage : notnull {
    readonly ConcurrentDictionary<Guid, IObserver<IStateChangedEventArgs<TMessage>>> _observers = new();

    public void StateHasChanged(TMessage? message = default) {
        InvokeObserver(new StateChangedEventArgs<TMessage>() {
            Message = message,
            Sender = this,
            StateChangeType = StateChangeType.StateHasChanged,
        });
    }

    void IStateObservable<TMessage>.StateHasChanged() {
        StateHasChanged(default);
    }

    public IDisposable Subscribe(IObserver<IStateChangedEventArgs<TMessage>> observer) {
        var id = Guid.NewGuid();
        if (_observers.TryAdd(id, observer) is false) {
            throw new InvalidOperationException("Failed to subscribe observer");
        }

        return new StoreSubscription(GetType().FullName ?? "StateObservable.Subscribe", () => {
            if (_observers.TryRemove(new(id, observer)) is false) {
                throw new InvalidOperationException("Failed to unsubscribe observer");
            }
        });
    }

    public IDisposable Subscribe(Action<IStateChangedEventArgs<TMessage>> observer) {
        return Subscribe(new GeneralObserver<IStateChangedEventArgs<TMessage>>(observer));
    }

    internal void InvokeObserver(IStateChangedEventArgs<TMessage> e) {
        foreach (var (_, obs) in _observers) {
            obs.OnNext(e);
        }
    }
}

public abstract record EditContext<TMessage> : IStateObservable<TMessage>
    where TMessage : notnull {
    readonly ConcurrentDictionary<Guid, IObserver<IStateChangedEventArgs<TMessage>>> _observers = new();

    public Ulid EditId { get; init; } = Ulid.NewUlid();

    public void StateHasChanged(TMessage? message = default) {
        InvokeObserver(new StateChangedEventArgs<TMessage>() {
            Message = message,
            Sender = this,
            StateChangeType = StateChangeType.StateHasChanged,
        });
    }

    void IStateObservable<TMessage>.StateHasChanged() {
        StateHasChanged(default);
    }

    public IDisposable Subscribe(IObserver<IStateChangedEventArgs<TMessage>> observer) {
        var id = Guid.NewGuid();
        if (_observers.TryAdd(id, observer) is false) {
            throw new InvalidOperationException("Failed to subscribe observer");
        }

        return new StoreSubscription(GetType().FullName ?? "EditContext.Subscribe", () => {
            if (_observers.TryRemove(new(id, observer)) is false) {
                throw new InvalidOperationException("Failed to unsubscribe observer");
            }
        });
    }

    public IDisposable Subscribe(Action<IStateChangedEventArgs<TMessage>> observer) {
        return Subscribe(new GeneralObserver<IStateChangedEventArgs<TMessage>>(observer));
    }


        internal void InvokeObserver(IStateChangedEventArgs<TMessage> e) {
        foreach (var (_, obs) in _observers) {
            obs.OnNext(e);
        }
    }
}
