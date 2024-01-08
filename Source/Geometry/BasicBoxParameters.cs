// Created by Hans-Jörg Schmid
// Licensed under MIT license

namespace BoxCreator.Geometry;

public record BasicBoxParameters(
    float BoxLength,
    float BoxWidth,
    float BoxHeight,
    float MaterialThickness,
    bool WithoutLid);
