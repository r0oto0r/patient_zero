using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapTileType {
    GRASS,
    WATER
}

public enum MapObjectType {
    NONE,
    BUILDING,
    GRASS
}

public class MapTile
{
    public  MapTileType tileType { get; set; }
    public int tileTypeMovementMalus { get; set; }

    public  MapObjectType objectType { get; set; }
    public int objectTypeMovementMalus { get; set; }
}
