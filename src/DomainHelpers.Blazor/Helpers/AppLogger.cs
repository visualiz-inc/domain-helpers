using DomainHelpers.Commons;
using DomainHelpers.Commons.Reactive;
using Microsoft.JSInterop;

namespace DomainHelpers.Blazor.Helpers;

public record LogEventArgs(Exception? Exception, string? Message, ImmutableArray<string>? Details, string? LogId, string DebugInfo);

public class AppLogger {
    private readonly ILogger<AppLogger> _logger;
    private readonly Subject<LogEventArgs> _subject = new();
    private readonly IJSRuntime _jsRuntime;

    public AppLogger(ILogger<AppLogger> logger, IJSRuntime jsRuntime) {
        _logger = logger;
        _jsRuntime = jsRuntime;
    }

    public void LogError(Exception ex, string message, bool notify = true) {
        if (ex is GeneralException ge) {
            LogError(ge, message+ "2222222222222");
        }
        else {
            _logger.LogError(ex, message);

            if (notify) {
                _subject.OnNext(new LogEventArgs(ex, message, null, null, ""));
            }
        }
    }

    public void LogError(GeneralException ex, string? message = null) {
        var details = new List<string>();
        var id = ex.EventId.ToString();
        var debugInfo = "";
        if (ex is RequestException failedResponseException) {
            _ = LogJson(failedResponseException);
            if (failedResponseException.FailedResponse is { } response) {
                debugInfo = response.Exception;
                details.Add(response.Title);
            }

            id = failedResponseException.FailedResponse?.EventId.ToString() ?? "";
        }

        details.AddRange(ex.FluttenDisplayMessages());
        _logger.LogError(ex, message);
        _subject.OnNext(new LogEventArgs(
            ex,
            message ?? ex.DisplayMessage,
            ArrayOfRange(details),
            id,
            debugInfo ?? ""
        ));
    }

    public async Task LogJson(object? obj) {
        await _jsRuntime.InvokeVoidAsync("console.log", obj);
    }

    public void LogError(string message) {
        _logger.LogError(message);
        _subject.OnNext(new LogEventArgs(null, message, ArrayOf<string>(), null, ""));
    }

    public IDisposable Subscribe(Action<LogEventArgs> observer) {
        return _subject.Subscribe(observer);
    }
}