// Created by Hans-Jörg Schmid
// Licensed under MIT license

namespace BoxCreator.Geometry;

public sealed class FingerJointBoxCreator
{
    public required BasicBoxParameters BasicBoxParameters { get; init; }
    public required FingerJointParameters FingerJointParameters { get; init; }

    public void CreateBox(string saveDxfFilePath)
    {
        if (FingerJointParameters.CornerReliefType == CornerReliefs.None)
        {
        }
    }
}
