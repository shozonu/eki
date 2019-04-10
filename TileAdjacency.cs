using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
enum TileAdjacency {
    None = 0x0,
    Left = 0x1,
    Right = 0x2,
    Up = 0x4,
    Down = 0x8,
    Above = 0x16,
    Below = 0x32
}
