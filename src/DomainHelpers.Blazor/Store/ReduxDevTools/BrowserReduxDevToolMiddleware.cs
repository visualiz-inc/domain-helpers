using DomainHelpers.Blazor.Store.Core;
using Microsoft.JSInterop;

namespace DomainHelpers.Blazor.Store.ReduxDevTools;

/// <summary>
/// A middleware that connect to Redux devtool.
/// </summary>
public sealed class BrowserReduxDevToolMiddleware : Middleware {
    readonly ReduxDevToolOption _chromiumDevToolOption;

    public BrowserReduxDevToolMiddleware(ReduxDevToolOption? chromiumDevToolOption = null) {
        _chromiumDevToolOption = chromiumDevToolOption ?? new();
    }

    protected override MiddlewareHandler Create(IServiceProvider provider) {
        var jsHandler = new JavaScriptDevToolInteropHandler(
            (IJSRuntime)(
                provider.GetService(typeof(IJSRuntime)) ?? throw new Exception()
            ),
            _chromiumDevToolOption
        );

        return new ReduxDevToolMiddlewareHandler(jsHandler, provider, _chromiumDevToolOption);
    }
}