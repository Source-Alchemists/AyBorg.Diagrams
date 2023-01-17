using AyBorg.Diagrams.Core.Geometry;
using AyBorg.Diagrams.Core.Models.Base;
using System.Linq;

namespace AyBorg.Diagrams.Core
{
    public static partial class Routers
    {
        public static Point[] Normal(Diagram _, BaseLinkModel link)
        {
            return link.Vertices.Select(v => v.Position).ToArray();
        }
    }
}
