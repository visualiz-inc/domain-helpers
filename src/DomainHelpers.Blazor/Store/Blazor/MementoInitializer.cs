using DomainHelpers.Blazor.Store.Core;
using Microsoft.AspNetCore.Components;

namespace DomainHelpers.Blazor.Store.Blazor;

public class MementoInitializer : ComponentBase, IDisposable {
    [Inject]
    public required StoreProvider StoreProvider { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            await StoreProvider.InitializeAsync();
        }
    }

    public void Dispose() {
        StoreProvider.Dispose();
    }
}