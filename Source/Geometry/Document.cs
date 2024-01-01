// Created by Hans-Jörg Schmid
// Licensed under MIT license

// Ignore Spelling: Dxf

using netDxf;

namespace BoxCreator.Geometry;

public class Document
{
    public void Clear()
    {
        _dxfDocument = new();
    }

    public bool SaveDxf(Stream stream)
    {
        return _dxfDocument.Save(stream, isBinary: false);
    }

    private DxfDocument _dxfDocument = new();
}
