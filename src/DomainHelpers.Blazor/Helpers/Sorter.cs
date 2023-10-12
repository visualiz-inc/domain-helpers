using DomainHelpers.Commons.Reactive;
using MudBlazor;

namespace DomainHelpers.Blazor.Helpers;

public record struct SorterItem<TColumn>(
    int Index,
    TColumn Column,
    SortDirection Direction
);

public readonly struct SorterState<TColumn> where TColumn : unmanaged, Enum {
    readonly IReadOnlyDictionary<TColumn, SortDirection>? _bag;
    readonly IList<TColumn>? _priorities;

    public int Count => _bag?.Count ?? 0;

    public SorterState(
        IReadOnlyDictionary<TColumn, SortDirection> bag,
        IList<TColumn> priorities
    ) {
        _bag = bag;
        _priorities = priorities;
    }

    public SortDirection this[TColumn key] {
        get => _bag?.TryGetValue(key, out var v) is true ? v : SortDirection.None;
    }

    public int? IndexOf(TColumn column) => _priorities?.IndexOf(column) switch {
        null or < 0 => null,
        var v => v + 1,
    };

    public IReadOnlyList<SorterItem<TColumn>> Sorts {
        get {
            if (_priorities is { } arr && _bag is { } b) {
                return arr.Select((x, i) => new SorterItem<TColumn>(i, x, b[x])).ToArray();
            }

            return [];
        }
    }
}

public class Sorter<TColumn>(bool _isMultiple = false) : IObservable<SorterState<TColumn>>
    where TColumn : unmanaged, Enum {
    readonly Dictionary<TColumn, SortDirection> _bag = new();
    readonly List<TColumn> _priorities = [];

    readonly Subject<SorterState<TColumn>> _subject = new();

    public SorterState<TColumn> State => new(
        new Dictionary<TColumn, SortDirection>(_bag),
        new List<TColumn>(_priorities)
    );

    public IDisposable Subscribe(Action<SorterState<TColumn>> action) =>
        _subject.Subscribe(action);

    public IDisposable Subscribe(IObserver<SorterState<TColumn>> observer) =>
        _subject.Subscribe(observer);

    public SortDirection this[TColumn key] {
        get => _bag.TryGetValue(key, out var v) ? v : SortDirection.None;
        set {
            if (_isMultiple is false) {
                _bag.Clear();
                _priorities.Clear();
            }

            if (_bag.ContainsKey(key)) {
                if (value == SortDirection.None) {
                    _bag.Remove(key);
                    _priorities.Remove(key);
                }
                else {
                    _bag[key] = value;
                }
            }
            else {
                _bag.Add(key, value);
                _priorities.Add(key);
            }

            _subject.OnNext(State);
        }
    }

    public void Clear() {
        _bag.Clear();
        _priorities.Clear();

        _subject.OnNext(State);
    }

    public IReadOnlyList<SorterItem<TColumn>> Sorts =>
        _priorities.Select((x, i) => new SorterItem<TColumn>(i, x, this[x]))
            .ToArray();
}