using Microsoft.JSInterop;

namespace DomainHelpers.Blazor.Helpers.JsInetrop; 
public class ScrollHelper {
    private const string FuncName = "ScrollTop";
    private const string Src = $$"""
    function {{FuncName}}(id) {
            var el = document.getElementById(id);
            el.scrollTop = 0;
        }        
        """;
    private readonly JSRuntime _jsRuntime;

    static ScrollHelper() {

    }

    public ScrollHelper(JSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    public async Task ScrollToTop() {
        await _jsRuntime.InvokeVoidAsync(FuncName);
    }
}