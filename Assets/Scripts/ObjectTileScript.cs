using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ObjectTileType {
    NONE,
    BUILDING,
    GRASS,
    TREE,
    CAR
}

public class ObjectTileScript : MapTileScript {
    public ObjectTileType type;
    public bool canBePlacedOnWater;
    public bool canBePlacedOnRoad;
    void Start() {

    }

    void Update() {
        
    }
}
