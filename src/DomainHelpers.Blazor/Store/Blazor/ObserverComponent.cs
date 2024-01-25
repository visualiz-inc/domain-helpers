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

    readonly ThrottledExecutor<IStateChangedEventArgs<object>> _stateHasChangedThrottler = new();
    readonly ConcurrentBag<IDisposable> _disposables = [];
    readonly ConcurrentBag<IWatcher> _watchers = [];

    /// <summary>
    /// Initializes a new instance of <see cref="ObserverComponent"/> class.
    /// </summary>
    public ObserverComponent() {
        AddDisposable(_stateHasChangedThrottler.Subscribe(e => {
            if (_isDisposed is false) {
                InvokeAsync(StateHasChanged);
            }
        }));
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
    void IDisposable.Dispose() {
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
        AddDisposable(StateSubscriber.Subscribe(this, e => {
            _stateHasChangedThrottler.LatencyMs = LatencyMs;
            _stateHasChangedThrottler.Invoke(e);
        }));

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
            foreach (var d in _disposables ?? []) {
                d.Dispose();
            }
            _disposables?.Clear();
        }
    }

    protected virtual void OnHandleEvents() {

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