using DomainHelpers.Commons;
using DomainHelpers.Commons.Reactive;
using Microsoft.JSInterop;

namespace DomainHelpers.Blazor.Helpers;

public record LogEventArgs(Exception? Exception, string? Message, ImmutableArray<string>? Details, string? LogId);

public class AppLogger {
    private readonly ILogger<AppLogger> _logger;
    private readonly Subject<LogEventArgs> _subject = new();
    private readonly IJSRuntime _jsRuntime;

    public AppLogger(ILogger<AppLogger> logger, IJSRuntime jsRuntime) {
        _logger = logger;
        _jsRuntime = jsRuntime;
    }

    public void LogError(Exception ex, string message) {
        if (ex is GeneralException ge) {
            LogError(ge, message);
        }
        else {
            _logger.LogError(ex, message);
            _subject.OnNext(new LogEventArgs(ex, message, null, null));
        }
    }

    public void LogError(GeneralException ex, string? message = null) {
        var details = new List<string>();
        if (ex is GeneralException<FailedResponse> failedResponseException) {
            _ = LogJson(failedResponseException.Payload);
            details.Add($"{failedResponseException.Payload}");
        }

        details.AddRange(ex.FluttenDisplayMessages());
        _logger.LogError(ex, message);
        _subject.OnNext(new LogEventArgs(
            ex,
            message ?? ex.DisplayMessage,
            ex.DisplayMessage is { }
                ? ArrayOf(ex.DisplayMessage).AddRange(details)
                : ArrayOfRange(details),
            ex.EventId.ToString()
         ));
    }

    public async Task LogJson(object? obj) {
        await _jsRuntime.InvokeVoidAsync("console.log", obj);
    }

    public void LogError(string message) {
        _logger.LogError(message);
        _subject.OnNext(new LogEventArgs(null, message, ArrayOf<string>(), null));
    }

    public IDisposable Subscribe(Action<LogEventArgs> observer) {
        return _subject.Subscribe(observer);
    }
}