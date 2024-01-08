// Created by Hans-Jörg Schmid
// Licensed under MIT license

namespace BoxCreator.Geometry;

public record BoxJointParameters(
    float MinJointSize,
    float JointAllowance,
    float EndmillDiameter,
    CornerReliefs CornerReliefType);
