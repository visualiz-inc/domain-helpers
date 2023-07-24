using Microsoft.JSInterop;

namespace DomainHelpers.Blazor.Helpers.JsInetrop; 
public class ClipBoardHelper {
    private readonly IJSRuntime _jsRuntime;

    public ClipBoardHelper(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    public async Task CopyAsync(string text) {
        await _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}