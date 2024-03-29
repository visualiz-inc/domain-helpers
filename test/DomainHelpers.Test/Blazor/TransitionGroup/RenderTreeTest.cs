using Bunit;
using DomainHelpers.Test.Blazor.TransitionGroup.Views;

namespace DomainHelper.Test.Blazor.TransitionGroup;

public class RenderTreeTest {
    [Fact]
    public void Ensure_RenderTree_is_Correct() {
        using var fake = new TestContext();
        var fakeRendered = fake.RenderComponent<FakeTransitionGroupCounter>();

        using var actual = new TestContext();
        var actualRendered = actual.RenderComponent<TransitionGroupCounter>();

        using var counterContext = new TestContext();
        var counterContextRendered = actual.RenderComponent<Counter>();

        Assert.NotEqual(fakeRendered.Markup, counterContextRendered.Markup);
    }
}