namespace DomainHelpers.Blazor.Store.Core; 
public class CommandNotHandledException(Command command) : Exception($"The command is not handled. {command.GetType().Name}") {
    public Command Command { get; } = command;
}