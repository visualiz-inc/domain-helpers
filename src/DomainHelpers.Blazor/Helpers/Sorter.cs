using DomainHelpers.Commons.Reactive;
using MudBlazor;
using System;
using System.Collections;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace DomainHelpers.Blazor.Helpers;
public record struct SorterItem<TColumn>(TColumn Column, SortDirection Direction)
    where TColumn : unmanaged, Enum;

public readonly struct SorterState<TColumn> : IReadOnlyCollection<SorterItem<TColumn>>
    where TColumn : unmanaged, Enum {
    readonly IReadOnlyDictionary<TColumn, SortDirection>? _bag;

    public int Count => _bag?.Count ?? 0;

    public SorterState(IReadOnlyDictionary<TColumn, SortDirection> bag) {
        _bag = bag;
    }

    public SortDirection this[TColumn key] {
        get => _bag?.TryGetValue(key, out var v) is true ? v : SortDirection.None;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return _bag?.Select(x => new SorterItem<TColumn>(x.Key, x.Value)).GetEnumerator()
            ?? Enumerable.Empty<SorterItem<TColumn>>().GetEnumerator();
    }

    IEnumerator<SorterItem<TColumn>> IEnumerable<SorterItem<TColumn>>.GetEnumerator() {
        return _bag?.Select(x => new SorterItem<TColumn>(x.Key, x.Value)).GetEnumerator()
            ?? Enumerable.Empty<SorterItem<TColumn>>().GetEnumerator();
    }
}

public class Sorter<TColumn>(bool _isMultiple = false) : IObservable<SorterState<TColumn>>
    where TColumn : unmanaged, Enum {
    readonly Dictionary<TColumn, SortDirection> _bag = new();
    readonly Subject<SorterState<TColumn>> _subject = new();

    public IDisposable Subscribe(Action<SorterState<TColumn>> action) =>
        _subject.Subscribe(action);

    public IDisposable Subscribe(IObserver<SorterState<TColumn>> observer) =>
        _subject.Subscribe(observer);

    public SortDirection this[TColumn key] {
        get => _bag.TryGetValue(key, out var v) ? v : SortDirection.None;
        set {
            if (_isMultiple is false) {
                _bag.Clear();
            }

            if (_bag.ContainsKey(key)) {
                _bag[key] = value;
            }
            else {
                _bag.Add(key, value);
            }

            _subject.OnNext(new(new Dictionary<TColumn, SortDirection>(_bag)));
        }
    }

    public IReadOnlyList<SorterItem<TColumn>> Sorts =>
        _bag.Select(x => new SorterItem<TColumn>(x.Key, x.Value))
            .ToArray();
}