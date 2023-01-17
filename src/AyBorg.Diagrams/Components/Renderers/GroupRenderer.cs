using AyBorg.Diagrams.Components.Groups;
using AyBorg.Diagrams.Core;
using AyBorg.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AyBorg.Diagrams.Components.Renderers
{
    public class GroupRenderer : ComponentBase
    {
        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public GroupModel Group { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var componentType = Diagram.GetComponentForModel(Group) ?? typeof(DefaultGroupWidget);
            builder.OpenComponent(0, componentType);
            builder.AddAttribute(1, "Group", Group);
            builder.CloseComponent();
        }
    }
}
