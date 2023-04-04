using DomainHelpers.Commons.Primitives;
using DomainHelpers.Commons.Reactive;
using System.Runtime.CompilerServices;

namespace DomainHelpers.Blazor.Helpers;

public readonly record struct LoadingState {
    private readonly ImmutableArray<string?> _loadings = ArrayOf<string?>();

    public LoadingState(ImmutableArray<string?> loadings) {
        _loadings = loadings;
    }

    public LoadingState() : this(ArrayOf<string?>()) { }

    public bool IsLoading => _loadings is not [];

    public bool IsGroupLoading(string? groupId) =>
        _loadings.Any(x => x == groupId);
}

public class Loading {
    private readonly Dictionary<Ulid, string?> _loadings = new();
    private readonly object _locker = new();
    private readonly Subject<LoadingState> _subject = new();

    public bool IsLoading {
        get {
            lock (_locker) {
                return _loadings.Count > 0;
            }
        }
    }

    public IDisposable Subscribe(Action<LoadingState> action) {
        return _subject.Subscribe(action);
    }

    public bool IsGroupLoading(string? groupId = default) {
        lock (_locker) {
            return _loadings.Values.Any(x => x == groupId);
        }
    }

    public IDisposable BeginLoading([CallerFilePath] string? groupId = default) {
        var id = Ulid.NewUlid();
        lock (_locker) {
            _loadings.Add(id, groupId);
        }

        _subject.OnNext(CreateData());

        return new Disposable(() => {
            lock (_locker) {
                _loadings.Remove(id);
            }

            _subject.OnNext(CreateData());
        });
    }

    private LoadingState CreateData() {
        lock (_locker) {
            return new LoadingState(
                _loadings.Values.ToImmutableArray()
            );
        }
    }

    private class Disposable : IDisposable {
        private readonly Action action;

        public Disposable(Action action) {
            this.action = action;
        }

        public void Dispose() {
            action();
        }
    }
}