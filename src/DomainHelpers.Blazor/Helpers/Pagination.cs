using DomainHelpers.Commons.Reactive;
using OneOf;

namespace DomainHelpers.Blazor.Helpers;

using PageStateChangedEvent = OneOf<CommitPage, ModifyTotalItems>;

public record CommitPage(PageState State);

public record ModifyTotalItems(PageState State);

public readonly record struct AvailablePageRange(int From, int To);

public readonly record struct OffsetFetch(int Offset, int Fetch);

public readonly record struct PageState {
    public int ItemsNumPerPage { get; init; }

    public int TotalItemsCount { get; init; }

    public int PageCount { get; init; }

    public AvailablePageRange CurrentItemsAvailable { get; init; }

    public int CurrentPage { get; init; }

    public OffsetFetch OffsetFetch { get; init; }
}

public class Pagination {
    private int _page = 1;
    private readonly Subject<PageStateChangedEvent> _subject = new();
    private int _totalItemsCount = 0;

    public PageState PageState => CreateState();

    public int ItemsNumPerPage { get; }

    public int TotalItemsCount {
        get => _totalItemsCount;
        set {
            _totalItemsCount = value;
            _subject.OnNext(new ModifyTotalItems(CreateState()));
        }
    }

    public int PageCount => (int)(Math.Floor((float)TotalItemsCount / ItemsNumPerPage) + 1);

    public AvailablePageRange CurrentItemsAvailable {
        get {
            var from = Math.Min(TotalItemsCount, (CurrentPage - 1) * ItemsNumPerPage + 1);
            var to = Math.Min(from + ItemsNumPerPage - 1, TotalItemsCount);
            return new(from, to);
        }
    }

    public int CurrentPage {
        get => _page;
        set {
            _page = Math.Max(0, Math.Min(value, PageCount));
            Commit();
        }
    }

    public OffsetFetch OffsetFetch => new(
        (CurrentPage - 1) * ItemsNumPerPage,
        ItemsNumPerPage
    );

    public Pagination(int itemsNumPerPage = 50) {
        ItemsNumPerPage = itemsNumPerPage;
    }

    public IDisposable Subscribe(Action<PageStateChangedEvent> action) => _subject.Subscribe(action);

    public void Reset() {
        TotalItemsCount = 0;
        CurrentPage = 1;
    }

    public void Next() => CurrentPage++;

    public void Prev() => CurrentPage--;

    public void Commit() {
        _subject.OnNext(new CommitPage(CreateState()));
    }

    private PageState CreateState() => new() {
        CurrentItemsAvailable = CurrentItemsAvailable,
        CurrentPage = CurrentPage,
        ItemsNumPerPage = ItemsNumPerPage,
        OffsetFetch = OffsetFetch,
        PageCount = PageCount,
        TotalItemsCount = TotalItemsCount,
    };
}