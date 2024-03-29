﻿namespace DomainHelpers.Blazor.TransitionGroup.Internal;
class TransitionGroupContext {
    public HashSet<object> AnimatingElements { get; } = [];

    public event Action<object>? RemoveAnimationRequested;

    public event Action? StateHasChanged;

    public TransitionGroupContext() {
    }

    public IDisposable SubscribeAnimationRequested(object elementkey, Action action) {
        void HandleRequested(object key) {
            if (key.Equals(elementkey)) {
                action();
            }
        }

        RemoveAnimationRequested += HandleRequested;
        return new Subscription(() => {
            RemoveAnimationRequested -= HandleRequested;
        });
    }

    public void BeginAnimation(object key) {
        if (AnimatingElements.Contains(key)) {
            return;
        }

        AnimatingElements.Add(key);
    }

    public void EndAnimation(object key) {
        if (AnimatingElements.Contains(key) is false) {
            return;
        }

        AnimatingElements.Remove(key);
    }

    public void RequestRemoveAnimation(object key) => RemoveAnimationRequested?.Invoke(key);

    public void RequestTransitionGroupStateHasChanged() => StateHasChanged?.Invoke();
}