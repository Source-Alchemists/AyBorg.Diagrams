using System.Collections.Generic;
using AyBorg.Diagrams.Core;
using AyBorg.Diagrams.Core.Geometry;
using AyBorg.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AyBorg.Diagrams.Components
{
    public partial class LinkWidget
    {
        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public LinkModel Link { get; set; }

        private void OnMouseDown(MouseEventArgs e, int index)
        {
            if (!Link.Segmentable)
                return;

            var vertex = CreateVertex(e.ClientX, e.ClientY, index);
            Diagram.OnMouseDown(vertex, e);
        }

        private void OnTouchStart(TouchEventArgs e, int index)
        {
            if (!Link.Segmentable)
                return;

            var vertex = CreateVertex(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY, index);
            Diagram.OnTouchStart(vertex, e);
        }

        private LinkVertexModel CreateVertex(double clientX, double clientY, int index)
        {
            var rPt = Diagram.GetRelativeMousePoint(clientX, clientY);
            var vertex = new LinkVertexModel(Link, rPt);
            Link.Vertices.Insert(index, vertex);
            return vertex;
        }

        private (Point source, Point target) FindConnectionPoints(Point[] route)
        {
            if (Link.SourcePort == null) // Portless
            {
                if (Link.SourceNode.Size == null || Link.TargetNode?.Size == null)
                    return (null, null);

                var sourceCenter = Link.SourceNode.GetBounds().Center;
                var targetCenter = Link.TargetNode?.GetBounds().Center ?? Link.OnGoingPosition;
                var firstPt = route.Length > 0 ? route[0] : targetCenter;
                var secondPt = route.Length > 0 ? route[0] : sourceCenter;
                var sourceLine = new Line(firstPt, sourceCenter);
                var targetLine = new Line(secondPt, targetCenter);
                var sourceIntersections = Link.SourceNode.GetShape().GetIntersectionsWithLine(sourceLine);
                var targetIntersections = Link.TargetNode.GetShape().GetIntersectionsWithLine(targetLine);
                var sourceIntersection = GetClosestPointTo(sourceIntersections, firstPt);
                var targetIntersection = GetClosestPointTo(targetIntersections, secondPt);
                return (sourceIntersection ?? sourceCenter, targetIntersection ?? targetCenter);
            }
            else
            {
                if (!Link.SourcePort.Initialized || Link.TargetPort?.Initialized == false)
                    return (null, null);

                var source = GetPortPositionBasedOnAlignment(Link.SourcePort, Link.SourceMarker);
                var target = GetPortPositionBasedOnAlignment(Link.TargetPort, Link.TargetMarker);
                return (source, target ?? Link.OnGoingPosition);
            }
        }

        private Point GetPortPositionBasedOnAlignment(PortModel port, LinkMarker marker)
        {
            if (port == null)
                return null;

            if (marker == null)
                return port.MiddlePosition;

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

        private Point GetClosestPointTo(IEnumerable<Point> points, Point point)
        {
            var minDist = double.MaxValue;
            Point minPoint = null;

            foreach (var pt in points)
            {
                var dist = pt.DistanceTo(point);
                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = pt;
                }
            }

            return minPoint;
        }
    }
}
