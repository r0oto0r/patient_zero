using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GroundTileType {
    GROUND,
    GRASS,
    WATER
}

public class GroundTileScript : MapTileScript {
    public  GroundTileType type;
    public bool isRoad;
    public bool isBuilding;
    public bool isCar;
    public bool isRiver;

    void Start() {

    }

    void Update() {
        
    }
}
