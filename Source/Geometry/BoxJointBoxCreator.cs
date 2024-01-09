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

        CreateBottomSide(document);
        document.SaveDxf(dxfFileStream);
    }

    private void CreateBottomSide(Document document)
    {
        var jointLayoutX = CalculateJointSize(BasicBoxParameters.BoxWidth, BoxJointParameters.MinJointSize);
        var jointLayoutY = CalculateJointSize(BasicBoxParameters.BoxLength, BoxJointParameters.MinJointSize);
        var startX = BasicBoxParameters.BoxHeight + SidesDistance;
        var startY = BasicBoxParameters.BoxHeight + SidesDistance;
        var currentPosition = new Vector2(startX, startY);
        var polyline = new Polyline(currentPosition);

        currentPosition = CreateLines(polyline, currentPosition, jointLayoutX, new Vector2(1, 0), new Vector2(0, 1));
        currentPosition = CreateLines(polyline, currentPosition, jointLayoutY, new Vector2(0, 1), new Vector2(-1, 0));
        currentPosition = CreateLines(polyline, currentPosition, jointLayoutX, new Vector2(-1, 0), new Vector2(0, -1));
        currentPosition = CreateLines(polyline, currentPosition, jointLayoutY, new Vector2(0, -1), new Vector2(1, 0));
        polyline.DeleteLastEntity();
        document.AddPolyline(polyline);
    }

    private static JointLayout CalculateJointSize(double sideLength, double minJointSize)
    {
        var jointCount = (int)Math.Floor(sideLength / minJointSize);

        if (0 == jointCount % 2)
        {
            jointCount--;
        }
        if (jointCount < 3)
        {
            jointCount = 3;
        }

        return (jointCount, sideLength / jointCount);
    }

    private Vector2 CreateLines(
        Polyline polyline,
        Vector2 startPosition,
        JointLayout jointLayout,
        Vector2 firstNormVector,
        Vector2 secondNormVector)
    {
        var nextPosition = startPosition;
        var firstVector = firstNormVector * jointLayout.JointSize;
        var secondVector = secondNormVector * BasicBoxParameters.MaterialThickness;

        for (var jointIndex = 1; jointIndex <= jointLayout.JointCount; jointIndex++)
        {
            if (BoxJointParameters.CornerReliefType == CornerReliefs.None)
            {
                nextPosition = AddSegmentToPolyline(
                    polyline,
                    nextPosition,
                    firstVector,
                    secondVector,
                    jointIndex == jointLayout.JointCount);
            }
            secondVector = -secondVector;
        }

        return nextPosition;
    }

    private static Vector2 AddSegmentToPolyline(
        Polyline polyline,
        Vector2 startPosition,
        Vector2 firstVector,
        Vector2 secondVector,
        bool ignoreSecondSegment)
    {
        var nextPosition = startPosition;

        nextPosition += firstVector;
        polyline.AddLine(nextPosition);
        if (!ignoreSecondSegment)
        {
            nextPosition += secondVector;
            polyline.AddLine(nextPosition);
        }

        return nextPosition;
    }

    private const double SidesDistance = 10;
}
