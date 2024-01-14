using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Blazor.Helpers.JsInetrop;

public class DownloadHelper (IJSRuntime jsRuntime){

    public async Task InitializeAsync() {
        var src = """
            function downloadFile(dataUrl, fileName) {
                var a = document.createElement('a');
                a.href = dataUrl;
                a.download = fileName;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            }
            """;

        await jsRuntime.InvokeVoidAsync("eval", src);
    }

    public async Task DownloadAsync(string text,string fileName,string mimeType = "text/csv") {
        var bytes = Encoding.UTF8.GetBytes(text);
        var base64 = Convert.ToBase64String(bytes);
        var dataUrl = $"data:{mimeType};base64,{base64}";

        await jsRuntime.InvokeVoidAsync("downloadFile", dataUrl, fileName);
    }
}
