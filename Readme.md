# Proposal: Allow base component classes to augment the generated BuildRenderTree output

Please note: This is related to https://github.com/aspnet/AspNetCore/issues/9857 but that is already closed, therefore a new proposal...

I am trying to add common functionality to a hierarchy of components by overriding the `BuildRenderTree` method in a common base class like this:

#### StyledComponentBase.cs
```
public abstract class StyledComponentBase : ComponentBase
{
    protected StyledComponentBase()
    {
    }

    public string BackgroundColor { get; set; } = "yellow";

    public string Style => $"background-color: {this.BackgroundColor};";

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "style", this.Style);
        builder.AddContent(2, $"Added by {this.GetType().Name}");
        builder.CloseElement();
    }
}
```
#### StyledParentComponent.razor
```
@inherits  StyledComponentBase

<h3>StyledParentComponent</h3>

@code {

}
```
But this scenario does not work, because the generated `StyledParentComponent` class' overriden `BuildRenderTree` method does not call the base class (as noted in https://github.com/aspnet/AspNetCore/issues/9857):
#### obj\Debug\netstandard2.0\Razor\Components\StyledParentComponent.razor.g.cs 
```
public class StyledParentComponent : StyledComponentBase
{
    #pragma warning disable 1998
    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
    {
        builder.AddMarkupContent(0, "<h3>StyledParentComponent</h3>");
    }
    #pragma warning restore 1998
}
```

It would be very useful IMHO, to add the possibility for base component classes to somehow augment the markup produced by derived classes generated from `.razor` files. For example `ComponentBase` could introduce two new `virtual` methods:
```
/// <summary>
/// In generated code, call to this method is inserted before the generated BuildRenderTree call.
/// </summary>
protected virtual void AugmentRenderTreeBeforeGeneratedContent(RenderTreeBuilder builder)
{
}

/// <summary>
/// In generated code, call to this method is inserted after the generated BuildRenderTree call.
/// </summary>
protected virtual void AugmentRenderTreeAfterGeneratedContent(RenderTreeBuilder builder)
{
}
```
And the `.razor` compiler would include calls to the methods in the generated code like this:
```
public class StyledParentComponent : StyledComponentBase
{
    #pragma warning disable 1998
    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
    {
        this.AugmentRenderTreeBeforeGeneratedContent(builder);
        builder.AddMarkupContent(0, "<h3>StyledParentComponent</h3>");
        this.AugmentRenderTreeAfterGeneratedContent(builder);
    }
    #pragma warning restore 1998
}
```
