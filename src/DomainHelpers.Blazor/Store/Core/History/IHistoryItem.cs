namespace DomainHelpers.Blazor.Store.Core.History; 
public interface IHistoryItem<out T> {
    string Name { get; }

    T HistoryState { get; }
}