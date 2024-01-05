// Created by Hans-Jörg Schmid
// Licensed under MIT license

// Ignore Spelling: Dxf Polyline

using netDxf;
using netDxf.Units;

namespace BoxCreator.Geometry;

public sealed class Document
{
    public Document()
    {
        _dxfDocument.DrawingVariables.InsUnits = DrawingUnits.Millimeters;
    }

    public void Clear()
    {
        _dxfDocument = new();
        _dxfDocument.DrawingVariables.InsUnits = DrawingUnits.Millimeters;
    }

    public void AddPolyline(Polyline polyline)
    {
        _dxfDocument.Entities.Add(polyline.DxfPolyline);
    }

    public bool SaveDxf(Stream stream)
    {
        return _dxfDocument.Save(stream, isBinary: false);
    }

    private DxfDocument _dxfDocument = new();
}
