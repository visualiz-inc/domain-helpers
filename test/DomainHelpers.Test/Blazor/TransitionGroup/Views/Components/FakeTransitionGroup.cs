using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DomainHelper.Test.Blazor.TransitionGroup.Views.Components;

public class FakeTransitionGroup : ComponentBase {
    /// <summary>
    /// The render fragment for ChildContent.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        base.BuildRenderTree(builder);
        builder.AddContent(0, ChildContent);
    }
}
