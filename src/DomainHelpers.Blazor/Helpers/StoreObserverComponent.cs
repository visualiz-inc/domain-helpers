using DomainHelpers.Blazor.Store.Blazor;

namespace DomainHelpers.Blazor.Helpers; 
public class StoreObserverComponent : ObserverComponent {
    ImmutableArray<IDisposable> _disposables = ImmutableArray<IDisposable>.Empty;

    protected override void OnInitialized() {
        base.OnInitialized();

        _disposables = OnHandleSubscribe().ToImmutableArray();
    }

    protected virtual IEnumerable<IDisposable> OnHandleSubscribe() {
        return Enumerable.Empty<IDisposable>();
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);

        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }

    class Disposable : IDisposable {
        readonly Action _action;

        public Disposable(Action action) {
            _action = action;
        }

        public void Dispose() {
            _action();
        }
    }
}