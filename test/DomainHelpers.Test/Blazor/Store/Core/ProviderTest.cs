using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Collections.Immutable;
using DomainHelpers.Blazor.Store.Blazor;
using DomainHelpers.Blazor.Store.Core;
using DomainHelper.Test.Blazor.Store.Core.Mock;

namespace DomainHelper.Test.Blazor.Store.Core;

public class ProviderTest {
    [Fact]
    public async Task Test() {
        var collection = new ServiceCollection();
        collection.AddMemento()
            .AddStore<Mock.AsyncCounterStore>();
        using var provider = collection.BuildServiceProvider();

        var store = provider.GetRequiredService<Mock.AsyncCounterStore>();
        var mementoProvider = provider.GetStoreProvider();

        var events = new List<RootStateChangedEventArgs>();

        using var subscription = mementoProvider.Subscribe(e => {
            events.Add(e);
        });

        await mementoProvider.InitializeAsync();

        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => {
            await mementoProvider.InitializeAsync();
        });

        store.CountUp();
        store.CountUp();
        store.CountUp();
        store.CountUp();
        store.CountUp();

        Assert.Equal(5, events.Count);

        // Ensure root state is correct
        var root = mementoProvider.CaptureRootState();
        var expected = JsonSerializer.Serialize(new {
            AsyncCounterStore = new AsyncCounterState {
                Count = 5,
                History = ImmutableArray.Create(1, 2, 3, 4, 5),
            }
        });
        var actual = JsonSerializer.Serialize(root.AsDictionary());
        Assert.Equal(expected, actual);
    }
}
