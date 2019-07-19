using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorClientApp.Components
{
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
    }
}
