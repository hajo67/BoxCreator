// Created by Hans-Jörg Schmid
// Licensed under MIT license

// Ignore Spelling: Dxf

using netDxf;

namespace BoxCreator.Geometry;

public sealed class BoxJointBoxCreator
{
    public required BasicBoxParameters BasicBoxParameters { get; init; }
    public required BoxJointParameters BoxJointParameters { get; init; }

    public void CreateBox(string saveDxfFilePath)
    {
        var document = new Document();
        var polyline = new Polyline(Vector2.Zero);
        using var dxfFileStream = new FileStream(
            saveDxfFilePath,
            FileMode.OpenOrCreate,
            FileAccess.Write);

        polyline.AddLine(new Vector2(400, 0));
        polyline.AddLine(new Vector2(400, 300));
        polyline.AddLine(new Vector2(0, 300));
        document.AddPolyline(polyline);
        document.SaveDxf(dxfFileStream);

        if (BoxJointParameters.CornerReliefType == CornerReliefs.None)
        {
        }
    }
}
