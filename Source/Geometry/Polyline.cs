// Created by Hans-Jörg Schmid
// Licensed under MIT license

// Ignore Spelling: Polyline Dxf

using netDxf;
using netDxf.Entities;

namespace BoxCreator.Geometry;

public sealed class Polyline
{
    public Polyline2D DxfPolyline => _polyline;

    public Polyline(Vector2 startPoint, bool isClosed = true)
    {
        _polyline.IsClosed = isClosed;
        _polyline.Vertexes.Add(new Polyline2DVertex(startPoint));
    }

    public void AddLine(Vector2 endPoint)
    {
        _polyline.Vertexes.Add(new Polyline2DVertex(endPoint));
    }

    public void AddArc(Vector2 endPoint, double bulge)
    {
        _polyline.Vertexes.Add(new Polyline2DVertex(endPoint, bulge));
    }

    public void AddPolyline(Polyline polyline, Vector2 translation)
    {
        foreach (var vertex in polyline.DxfPolyline.Vertexes)
        {
            _polyline.Vertexes.Add(new Polyline2DVertex(
                vertex.Position + translation,
                vertex.Bulge));
        }
    }

    private readonly Polyline2D _polyline = new();
}
