﻿using Microsoft.JSInterop;

namespace DomainHelpers.Blazor.Store.ReduxDevTools;
/// <summary>
/// Represents the Redux Developer Tool middleware used for debugging and profiling Redux stores on Browser.
/// Interact with ReduxDevTools via JavaScript interop on `Microsoft.JSInterop`.
/// </summary>
public sealed class BrowserReduxDevToolMiddleware : Middleware {
    readonly ReduxDevToolOption _chromiumDevToolOption;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserReduxDevToolMiddleware" /> class with the specified options and interop handler.
    /// </summary>
    /// <param name="chromiumDevToolOption">The configuration options for the Redux Developer Tool middleware (optional).</param>
    public BrowserReduxDevToolMiddleware(ReduxDevToolOption? chromiumDevToolOption = null) {
        _chromiumDevToolOption = chromiumDevToolOption ?? new();
    }

    /// <summary>
    /// Creates a new <see cref="ReduxDevToolMiddlewareHandler"/> instance for the Redux Developer Tool middleware.
    /// </summary>
    /// <param name="provider">The service provider used to resolve dependencies.</param>
    /// <returns>A new <see cref="ReduxDevToolMiddlewareHandler"/> instance for the Redux Developer Tool middleware.</returns>
    protected override ReduxDevToolMiddlewareHandler Create(IServiceProvider provider) {
       var p = provider.GetServices<IJSRuntime>();
      //  p.InvokeVoidAsync("console.log", "AAAAAAAAAAAAA");

        Console.WriteLine(p);

        var jsHandler = new JavaScriptDevToolInteropHandler(
            (IJSRuntime)(
                provider.GetService(typeof(IJSRuntime)) ?? throw new Exception()
            ),
            _chromiumDevToolOption
        );

        return new ReduxDevToolMiddlewareHandler(jsHandler, provider, _chromiumDevToolOption);
    }
}