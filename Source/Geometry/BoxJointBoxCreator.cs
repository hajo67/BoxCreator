// Created by Hans-Jörg Schmid
// Licensed under MIT license

// Ignore Spelling: Dxf

using netDxf;

namespace BoxCreator.Geometry;

using JointLayout = (int JointCount, double JointSize);

public sealed class BoxJointBoxCreator
{
    public required BasicBoxParameters BasicBoxParameters { get; init; }
    public required BoxJointParameters BoxJointParameters { get; init; }

    public void CreateBox(string saveDxfFilePath)
    {
        var document = new Document();
        using var dxfFileStream = new FileStream(
            saveDxfFilePath,
            FileMode.OpenOrCreate,
            FileAccess.Write);

        CreateBottom(document);
        CreateFront(document);
        CreateBack(document);
        CreateLeft(document);
        CreateRight(document);
        CreateTop(document);
        document.SaveDxf(dxfFileStream);
    }

    private void CreateBottom(Document document)
    {
        var startX = BasicBoxParameters.BoxHeight + SidesDistance;
        var startY = BasicBoxParameters.BoxHeight + SidesDistance;
        var faceCreator = new BoxJointFaceCreator()
        {
            JointFaceSides = FaceSides.All,
            Origin = new Vector2(startX, startY),
            Length = BasicBoxParameters.BoxLength,
            Height = BasicBoxParameters.BoxWidth,
            Thickness = BasicBoxParameters.MaterialThickness,
            BoxJointParameters = BoxJointParameters,
            StartWithThicknessOffsetInLengthDirection = false,
            StartWithThicknessOffsetInHeightDirection = false
        };
        var polyline = faceCreator.CreateFace();
        document.AddPolyline(polyline);
    }

    private void CreateFront(Document document)
    {
        var startX = BasicBoxParameters.BoxHeight + SidesDistance;
        var startY = 0.0;
        var faceSides = BasicBoxParameters.WithoutTopSide ?
            GetJointSidesMinusSide(FaceSides.Bottom) :
            FaceSides.All;

        var faceCreator = new BoxJointFaceCreator()
        {
            JointFaceSides = faceSides,
            Origin = new Vector2(startX, startY),
            Length = BasicBoxParameters.BoxLength,
            Height = BasicBoxParameters.BoxHeight,
            Thickness = BasicBoxParameters.MaterialThickness,
            BoxJointParameters = BoxJointParameters,
            StartWithThicknessOffsetInLengthDirection = false,
            StartWithThicknessOffsetInHeightDirection = true
        };
        var polyline = faceCreator.CreateFace();
        document.AddPolyline(polyline);
    }

    private void CreateBack(Document document)
    {
        var startX = BasicBoxParameters.BoxHeight + SidesDistance;
        var startY = BasicBoxParameters.BoxHeight + 2 * SidesDistance + BasicBoxParameters.BoxWidth;
        var faceSides = BasicBoxParameters.WithoutTopSide ?
            GetJointSidesMinusSide(FaceSides.Top) :
            FaceSides.All;

        var faceCreator = new BoxJointFaceCreator()
        {
            JointFaceSides = faceSides,
            Origin = new Vector2(startX, startY),
            Length = BasicBoxParameters.BoxLength,
            Height = BasicBoxParameters.BoxHeight,
            Thickness = BasicBoxParameters.MaterialThickness,
            BoxJointParameters = BoxJointParameters,
            StartWithThicknessOffsetInLengthDirection = false,
            StartWithThicknessOffsetInHeightDirection = true
        };
        var polyline = faceCreator.CreateFace();
        document.AddPolyline(polyline);
    }

    private void CreateLeft(Document document)
    {
        var startX = 0.0;
        var startY = BasicBoxParameters.BoxHeight + SidesDistance;
        var faceSides = BasicBoxParameters.WithoutTopSide ?
            GetJointSidesMinusSide(FaceSides.Right) :
            FaceSides.All;

        var faceCreator = new BoxJointFaceCreator()
        {
            JointFaceSides = faceSides,
            Origin = new Vector2(startX, startY),
            Length = BasicBoxParameters.BoxHeight,
            Height = BasicBoxParameters.BoxWidth,
            Thickness = BasicBoxParameters.MaterialThickness,
            BoxJointParameters = BoxJointParameters,
            StartWithThicknessOffsetInLengthDirection = true,
            StartWithThicknessOffsetInHeightDirection = false
        };
        var polyline = faceCreator.CreateFace();
        document.AddPolyline(polyline);
    }

    private void CreateRight(Document document)
    {
        var startX = BasicBoxParameters.BoxHeight + 2 * SidesDistance + BasicBoxParameters.BoxLength;
        var startY = BasicBoxParameters.BoxHeight + SidesDistance;
        var faceSides = BasicBoxParameters.WithoutTopSide ?
            GetJointSidesMinusSide(FaceSides.Left) :
            FaceSides.All;

        var faceCreator = new BoxJointFaceCreator()
        {
            JointFaceSides = faceSides,
            Origin = new Vector2(startX, startY),
            Length = BasicBoxParameters.BoxHeight,
            Height = BasicBoxParameters.BoxWidth,
            Thickness = BasicBoxParameters.MaterialThickness,
            BoxJointParameters = BoxJointParameters,
            StartWithThicknessOffsetInLengthDirection = true,
            StartWithThicknessOffsetInHeightDirection = false
        };
        var polyline = faceCreator.CreateFace();
        document.AddPolyline(polyline);
    }

    private void CreateTop(Document document)
    {
        if (!BasicBoxParameters.WithoutTopSide)
        {
            var startX = BasicBoxParameters.BoxHeight + SidesDistance;
            var startY = 2 * BasicBoxParameters.BoxHeight + 3 * SidesDistance + BasicBoxParameters.BoxWidth;
            var faceCreator = new BoxJointFaceCreator()
            {
                JointFaceSides = FaceSides.All,
                Origin = new Vector2(startX, startY),
                Length = BasicBoxParameters.BoxLength,
                Height = BasicBoxParameters.BoxWidth,
                Thickness = BasicBoxParameters.MaterialThickness,
                BoxJointParameters = BoxJointParameters,
                StartWithThicknessOffsetInLengthDirection = false,
                StartWithThicknessOffsetInHeightDirection = false
            };
            var polyline = faceCreator.CreateFace();
            document.AddPolyline(polyline);
        }
    }

    private static FaceSides GetJointSidesMinusSide(FaceSides minusSide)
    {
        return FaceSides.All ^ minusSide;
    }

    private const double SidesDistance = 10;
}
