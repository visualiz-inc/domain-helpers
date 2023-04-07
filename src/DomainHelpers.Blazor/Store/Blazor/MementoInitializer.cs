using DomainHelpers.Blazor.Store.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace DomainHelpers.Blazor.Store.Blazor;

public class MementoInitializer : ComponentBase, IDisposable {
    [Inject]
    public required IServiceProvider StoreProvider { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            await StoreProvider.GetRequiredService<StoreProvider>().InitializeAsync();
        }
    }

    public void Dispose() {
         StoreProvider.GetRequiredService<StoreProvider>().Dispose();
    }
}