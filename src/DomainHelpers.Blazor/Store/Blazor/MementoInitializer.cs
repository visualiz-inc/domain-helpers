using Microsoft.AspNetCore.Components;

namespace DomainHelpers.Blazor.Store.Blazor;
/// <summary>
/// The component for initializing Memento instance.
/// </summary>
public class MementoInitializer : ComponentBase, IDisposable {
    [Inject]
    public required StoreProvider StoreProvider { get; set; }

    [Parameter]
    public EventCallback Initialized { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            await StoreProvider.InitializeAsync();
            await Initialized.InvokeAsync();
        }
    }

    public void Dispose() {
        StoreProvider.Dispose();
    }
}