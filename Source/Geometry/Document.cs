// Created by Hans-Jörg Schmid
// Licensed under MIT license

// Ignore Spelling: Dxf Polyline

using netDxf;

namespace BoxCreator.Geometry;

public class Document
{
    public void Clear()
    {
        _dxfDocument = new();
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
