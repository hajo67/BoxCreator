// Created by Hans-Jörg Schmid
// Licensed under MIT license

using netDxf;

namespace BoxCreator.Geometry;

using JointLayout = (int JointCount, double JointSize);

internal sealed class BoxJointFaceCreator
{
    public required FaceSides JointFaceSides { get; set; }
    public required Vector2 Origin { get; set; }
    public required double Length { get; set; }
    public required double Height { get; set; }
    public required double Thickness { get; set; }
    public required BoxJointParameters BoxJointParameters { get; set; }
    public required bool StartWithThicknessOffsetInLengthDirection { get; set; }
    public required bool StartWithThicknessOffsetInHeightDirection { get; set; }

    public Polyline CreateFace()
    {
        (var length, var height, var currentPosition) = AdaptWidthAndHeightAndOrigin();
        var jointLayoutX = CalculateJointSize(length, BoxJointParameters.MinJointSize);
        var jointLayoutY = CalculateJointSize(height, BoxJointParameters.MinJointSize);
        var polyline = new Polyline(currentPosition);
        var firstVector = Vector2.UnitX;
        var secondVector = StartWithThicknessOffsetInHeightDirection ? -Vector2.UnitY : Vector2.UnitY;
        currentPosition = JointFaceSides.HasFlag(FaceSides.Bottom) ?
            CreateLines(polyline, currentPosition, jointLayoutX, firstVector, secondVector) :
            CreateSingleLine(polyline, currentPosition, jointLayoutX, firstVector);
        firstVector = Vector2.UnitY;
        secondVector = StartWithThicknessOffsetInLengthDirection ? Vector2.UnitX : -Vector2.UnitX;
        currentPosition = JointFaceSides.HasFlag(FaceSides.Right) ?
            CreateLines(polyline, currentPosition, jointLayoutY, firstVector, secondVector) :
            CreateSingleLine(polyline, currentPosition, jointLayoutY, firstVector);
        firstVector = -Vector2.UnitX;
        secondVector = StartWithThicknessOffsetInHeightDirection ? Vector2.UnitY : -Vector2.UnitY;
        currentPosition = JointFaceSides.HasFlag(FaceSides.Top) ?
            CreateLines(polyline, currentPosition, jointLayoutX, firstVector, secondVector) :
            CreateSingleLine(polyline, currentPosition, jointLayoutX, firstVector);
        firstVector = -Vector2.UnitY;
        secondVector = StartWithThicknessOffsetInLengthDirection ? -Vector2.UnitX : Vector2.UnitX;
        _ = JointFaceSides.HasFlag(FaceSides.Left) ?
            CreateLines(polyline, currentPosition, jointLayoutY, firstVector, secondVector) :
            CreateSingleLine(polyline, currentPosition, jointLayoutY, firstVector);
        polyline.DeleteLastEntity();

        return polyline;
    }

    private (double Length, double Height, Vector2 Origin) AdaptWidthAndHeightAndOrigin()
    {
        var origin = new Vector2(Origin.X, Origin.Y);
        var length = Length;
        var height = Height;

        if (StartWithThicknessOffsetInLengthDirection)
        {
            length = ReduceValueByThicknessDependingOnJointFaceSides(Length, FaceSides.Left, FaceSides.Right);
            origin.X += JointFaceSides.HasFlag(FaceSides.Left) ?
                Thickness :
                0;
        }
        if (StartWithThicknessOffsetInHeightDirection)
        {
            height = ReduceValueByThicknessDependingOnJointFaceSides(Height, FaceSides.Bottom, FaceSides.Top);
            origin.Y += JointFaceSides.HasFlag(FaceSides.Bottom) ?
                Thickness :
                0;
        }

        return (length, height, origin);
    }

    private double ReduceValueByThicknessDependingOnJointFaceSides(
        double value,
        FaceSides firstSide,
        FaceSides secondSide)
    {
        if (JointFaceSides.HasFlag(firstSide))
        {
            value -= Thickness;
        }
        if (JointFaceSides.HasFlag(secondSide))
        {
            value -= Thickness;
        }

        return value;
    }

    private static JointLayout CalculateJointSize(
        double sideLength,
        double minJointSize)
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
        var secondVector = secondNormVector * Thickness;

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

    private static Vector2 CreateSingleLine(
        Polyline polyline,
        Vector2 startPosition,
        JointLayout jointLayout,
        Vector2 normVector)
    {
        var lineLength = jointLayout.JointSize * jointLayout.JointCount;
        var endPosition = startPosition + lineLength * normVector;

        polyline.AddLine(endPosition);

        return endPosition;
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
}
