using System;
[Flags]
public enum Neighbours
{
    NONE = 0,
    HORIZONTAL = 1, //0001
    VERTICAL = 2, // 0010
    DIAGONALS = 4, // 0100
    CROSS = HORIZONTAL | VERTICAL,
    ALL = CROSS | DIAGONALS
}

