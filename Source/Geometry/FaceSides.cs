// Created by Hans-Jörg Schmid
// Licensed under MIT license

namespace BoxCreator.Geometry;

[Flags]
internal enum FaceSides
{
    None = 0b0000000,
    Left = 0b0000001,
    Right = 0b0000010,
    Bottom = 0b0000100,
    Top = 0b0001000,
    All = 0b0001111
}
