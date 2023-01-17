using System.Collections.Generic;

namespace AyBorg.Diagrams.Core.Geometry
{
    public interface IShape
    {
        public IEnumerable<Point> GetIntersectionsWithLine(Line line);
    }
}
