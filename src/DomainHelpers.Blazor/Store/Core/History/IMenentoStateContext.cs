namespace DomainHelpers.Blazor.Store.Core.History;

public interface IMementoStateContext {
    string Name { get; }
    object? State { get; set; }
}

public interface IMementoStateContext<T> : IMementoStateContext {
    new T? State { get; set; }
}