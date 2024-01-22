using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;

namespace DomainHelpers.Blazor.Store.Blazor;

/// <summary>
/// The base class for components that observe state changes in a store.
/// Injected stores that implements <see cref="IStore"/> interface will all be subscribed state change events
/// and call <see cref="ComponentBase.StateHasChanged"/> automatically.
/// </summary>
public class ObserverComponent : ComponentBase, IDisposable {
    private bool _isDisposed;
    private IDisposable? _stateSubscription;

    readonly IDisposable _invokerSubscription;
    readonly ThrottledExecutor<IStateChangedEventArgs<object>> _stateHasChangedThrottler = new();
    readonly ConcurrentBag<IDisposable> _disposables = new();
    readonly ConcurrentBag<IWatcher> _watchers = new();

    /// <summary>
    /// Initializes a new instance of <see cref="ObserverComponent"/> class.
    /// </summary>
    public ObserverComponent() {
        _invokerSubscription = _stateHasChangedThrottler.Subscribe(e => {
            if (_isDisposed is false) {
                InvokeAsync(StateHasChanged);
            }
        });
    }

    /// <summary>
    /// If greater than 0, the feature will not execute state changes
    /// more often than this many times per second. Additional notifications
    /// will be suppressed, and observers will be notified of the last.
    /// state when the time window has elapsed to allow another notification.
    /// </summary>
    protected ushort LatencyMs { get; set; } = 100;

    /// <summary>
    /// Disposes of the component and unsubscribes from any state
    /// </summary>
    public void Dispose() {
        if (_isDisposed is false) {
            Dispose(true);
            GC.SuppressFinalize(this);
            _isDisposed = true;
        }
    }

    /// <summary>
    /// Subscribes to state properties.
    /// </summary>
    protected sealed override void OnInitialized() {
        base.OnInitialized();
        _stateSubscription = StateSubscriber.Subscribe(this, e => {
            _stateHasChangedThrottler.LatencyMs = LatencyMs;
            _stateHasChangedThrottler.Invoke(e);
        });



        foreach (var d in OnHandleDisposable()) {
            _disposables.Add(d);
        }

        foreach (var w in OnHandleWatchers(new())) {
            _watchers.Add(w);
        }

        Initialized();
    }

    /// <inheritdoc />
    protected sealed override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);

        foreach (var watcher in _watchers) {
            watcher.InvokeOnParameterSet();
        }

        AfterRender(firstRender);
    }

    protected virtual void Initialized() {

    }

    protected virtual void AfterRender(bool firstRender) {

    }

    /// <summary>
    /// It will be called when 
    /// </summary>
    /// <param name="disposing"></param>
    /// <exception cref="NullReferenceException">Throws when you forgot to call base.InitializeAsync().</exception>
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            if (_stateSubscription is null) {
                throw new NullReferenceException("Have you forgotten to call base.InitializeAsync() in your component?");
            }

            _invokerSubscription.Dispose();
            _stateSubscription.Dispose();

            foreach (var d in _disposables ?? []) {
                d.Dispose();
            }
        }
    }

    /// <summary>
    /// Handles IDisposable. Generated disposables will be Disposed when the component is destroyed
    /// </summary>
    /// <returns>The disposables.</returns>

    [Obsolete]
    protected virtual IEnumerable<IDisposable> OnHandleDisposable() {
        return [];
    }

    [Obsolete]
    protected virtual IEnumerable<IWatcher> OnHandleWatchers(WatcherFactory watcherFactory) {
        return [];
    }

    protected virtual void OnHandleEvents( ) {

    }

    protected void AddDisposable(IDisposable disposable) {
        _disposables.Add(disposable);
    }

    protected void AddDisposables(IEnumerable<IDisposable> disposables) {
        foreach (var item in disposables) {
            _disposables.Add(item);
        }
    }

    protected void Watch<T>(Func<T> selector, Action<T> action, bool once = false) {
        _watchers.Add(new Watcher<T>(action, selector, once));
    }
}

public class WatcherFactory {
    public IWatcher Watch<T>(Action<T> action, Func<T> selector, bool once = false) {
        return new Watcher<T>(action, selector, once);
    }
}

public interface IWatcher {
    internal void InvokeOnParameterSet();
}

internal class Watcher<T>(Action<T> action, Func<T> selector, bool once = false) : IWatcher {
    T? _last = default;
    bool _invoked;

    void IWatcher.InvokeOnParameterSet() {
        if (once && _invoked) {
            return;
        }

        var val = selector.Invoke();
        if (val?.Equals(_last) is false) {
            _invoked = true;
            _last = val;
            action(val);
        }
    }
}