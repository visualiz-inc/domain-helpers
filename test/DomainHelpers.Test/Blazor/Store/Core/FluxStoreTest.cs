﻿using DomainHelper.Test.Blazor.Store.Core.Mock;
using DomainHelpers.Blazor.Store.Core;

namespace DomainHelper.Test.Blazor.Store.Core;

public class FluxStoreTest {
    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task Ensure_StateChanges(int count) {
        var store = new FluxAsyncCounterStore();

        var list = new List<int>();

        var actual = 0;
        for (var i = 0; i < count; i++) {

            await store.CountUpAsync();
            actual++;
            list.Add(actual);

            Assert.Equal(actual, store.State.Count);
            Assert.Equal(list, store.State.History);

            store.SetCount(1234);
            actual = 1234;
            list.Add(actual);

            Assert.Equal(actual, store.State.Count);
            Assert.Equal(list, store.State.History);
        }

    }

    [Theory]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(100)]
    public async Task Ensure_CanChangeStateAsync(int count) {
        var store = new FluxAsyncCounterStore();

        var list = new List<int>();
        for (var i = 0; i < count; i++) {
            list.Add(i);
            var task = store.SetCountWithRandomTimerAsync(i);
            Assert.True(store.State.IsLoading);
            await task;
            Assert.False(store.State.IsLoading);
            Assert.Equal(i, store.State.Count);
            Assert.Equal(list, store.State.History);
        }
    }

    [Fact]
    public async Task Command_CouldBeSubscribeCorrectly() {
        var store = new FluxAsyncCounterStore();

        var commands = new List<Command>();
        var lastState = store.State;
        using var subscription = store.Subscribe(e => {
            Assert.Equal(e.Sender, store);
            Assert.NotEqual(e.State, lastState);
            Assert.Equal(e.LastState, lastState);
            lastState = e.State;
            commands.Add(e.Command);
        });

        await store.CountUpAsync();
        store.SetCount(1234);
        await store.CountUpAsync();

        Assert.True(commands is [
            FluxAsyncCounterCommands.BeginLoading,
            FluxAsyncCounterCommands.Increment,
            FluxAsyncCounterCommands.EndLoading,
            FluxAsyncCounterCommands.ModifyCount(1234),
            FluxAsyncCounterCommands.BeginLoading,
            FluxAsyncCounterCommands.Increment,
            FluxAsyncCounterCommands.EndLoading
        ]);
    }

    [Fact]
    public void Ensure_StateHasChangedInvoked() {
        var store = new FluxAsyncCounterStore();
        var commands = new List<Command>();

        var lastState = store.State;
        using var subscription = store.Subscribe(e => {
            commands.Add(e.Command);
        });

        store.StateHasChanged();
        store.StateHasChanged();
        store.StateHasChanged();
        store.StateHasChanged();
        store.StateHasChanged();
        store.StateHasChanged();

        Assert.True(commands is [
            Command.StateHasChanged,
            Command.StateHasChanged,
            Command.StateHasChanged,
            Command.StateHasChanged,
            Command.StateHasChanged,
            Command.StateHasChanged,
        ]);
    }
}
