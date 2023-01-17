using AyBorg.Diagrams.Core.Geometry;
using AyBorg.Diagrams.Core.Models;

namespace AyBorg.Diagrams.Core
{
    public static partial class Routers
    {
        public static Point GetPortPositionBasedOnAlignment(PortModel port)
        {
            var pt = port.Position;
            return port.Alignment switch
            {
                PortAlignment.Top => new Point(pt.X + port.Size.Width / 2, pt.Y),
                PortAlignment.TopRight => new Point(pt.X + port.Size.Width, pt.Y),
                PortAlignment.Right => new Point(pt.X + port.Size.Width, pt.Y + port.Size.Height / 2),
                PortAlignment.BottomRight => new Point(pt.X + port.Size.Width, pt.Y + port.Size.Height),
                PortAlignment.Bottom => new Point(pt.X + port.Size.Width / 2, pt.Y + port.Size.Height),
                PortAlignment.BottomLeft => new Point(pt.X, pt.Y + port.Size.Height),
                PortAlignment.Left => new Point(pt.X, pt.Y + port.Size.Height / 2),
                PortAlignment.TopLeft => null!,
                _ => pt,
            };
        }
    }
}
