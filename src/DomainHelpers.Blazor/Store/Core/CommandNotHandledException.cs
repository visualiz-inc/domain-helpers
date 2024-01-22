namespace DomainHelpers.Blazor.Store.Core;
public class CommandNotHandledException<TMessage>(TMessage command)
    : Exception($"The command is not handled. {command?.GetType().Name}")
    where TMessage : notnull {
    public TMessage Command { get; } = command;
}