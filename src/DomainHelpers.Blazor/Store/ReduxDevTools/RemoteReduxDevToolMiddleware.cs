﻿namespace DomainHelpers.Blazor.Store.ReduxDevTools;
/// <summary>
/// Represents the Redux Developer Tool middleware used for debugging and profiling Redux stores via WebSocket.
/// Connect applications such as Blazor Hybrid, Native Application, and Blazor Server that do not directly use a browser to Redux Dev Tools.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RemoteReduxDevToolMiddleware" /> class with the specified options and interop handler.
/// </remarks>
/// <param name="devToolOption">The configuration options for the Redux Developer Tool middleware (optional).</param>
public sealed class RemoteReduxDevToolMiddleware(ReduxDevToolOption? devToolOption = null) : Middleware {
    readonly ReduxDevToolOption _chromiumDevToolOption = devToolOption ?? new();

    protected override MiddlewareHandler Create(IServiceProvider provider) {
        var webSocketConnection = (DevToolWebSocketConnection?)provider.GetService(typeof(DevToolWebSocketConnection))
            ?? new DevToolWebSocketConnection();
        var handler = new RemoteDevToolInteropHandler(webSocketConnection);
        return new ReduxDevToolMiddlewareHandler(handler, provider, _chromiumDevToolOption);
    }
}