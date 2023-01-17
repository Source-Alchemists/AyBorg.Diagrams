using AyBorg.Diagrams.Core.Geometry;
using AyBorg.Diagrams.Core.Models;
using AyBorg.Diagrams.Core.Models.Base;

namespace AyBorg.Diagrams.Core
{
    public delegate Point[] Router(Diagram diagram, BaseLinkModel link);

    public delegate PathGeneratorResult PathGenerator(Diagram diagram, BaseLinkModel link, Point[] route, Point source, Point target);

    public delegate BaseLinkModel LinkFactory(Diagram diagram, PortModel sourcePort);

    public delegate GroupModel GroupFactory(Diagram diagram, NodeModel[] children);
}
