using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Blazor.Helpers.JsInetrop;

public class DownloadHelper(IJSRuntime jsRuntime) {
    bool _hasCalled = false;
    public async Task InitializeAsync() {
        var src = """
            function downloadFile(dataUrl, fileName) {
                var a = document.createElement('a');
                a.href = dataUrl;
                a.download = fileName;
                a.click();
            }
            """;

        await jsRuntime.InvokeVoidAsync("eval", src);
    }

    public async Task DownloadAsync(string text, string fileName, string mimeType = "text/csv") {
        if (_hasCalled is false) {
            _hasCalled = true;
            await InitializeAsync();
        }

        var bom = new byte[] { 0xEF, 0xBB, 0xBF };
        var textBytes = Encoding.UTF8.GetBytes(text);
        var base64 = Convert.ToBase64String([.. bom, .. textBytes]);
        var dataUrl = $"data:{mimeType};charset=utf-8;base64,{base64}";

        await jsRuntime.InvokeVoidAsync("downloadFile", dataUrl, fileName);
    }
}
