using System;

namespace Slice
{
    [Flags]
    public enum SliceType
    {
        None = 0,
        RightUp = 1,
        Right = 2,
        RightDown = 4,
        LeftDown = 8,
        Left = 16,
        LeftUp = 32,
        Max = 63,
    }
}