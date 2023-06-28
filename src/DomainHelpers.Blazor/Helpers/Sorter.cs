using MudBlazor;

namespace DomainHelpers.Blazor.Helpers;

public record Sort<TColumn>(TColumn Column, SortDirection Direction)
    where TColumn : unmanaged, Enum;

public class Sorter<TColumn> : Dictionary<TColumn, SortDirection>
    where TColumn : unmanaged, Enum {
    readonly bool _isMultiple = false;

    public Sorter(bool isMultiple = false) {
        _isMultiple = isMultiple;
    }

    public new SortDirection this[TColumn key] {
        get => TryGetValue(key, out var v) ? v : default!;
        set {
            if (_isMultiple is false) {
                Clear();
            }

            if (ContainsKey(key)) {
                this[key] = value;
            }
            else {
                Add(key, value);
            }
        }
    }

    public ImmutableArray<Sort<TColumn>> Sorts =>
        this.Select(x => new Sort<TColumn>(x.Key, x.Value))
            .ToImmutableArray();
}
