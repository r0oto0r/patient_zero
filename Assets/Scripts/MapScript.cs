using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public GameObject groundLayer;
    public GameObject firstLayer;
    public GameObject secondLayer;
	public TileBase grassTileBase;
    public TileBase envGrassTileBase;
    public TileBase[] roadTileBases;
    public TileBase[] waterTileBases;
    public int mapWidth = 256;
    public int mapHeight = 256;
    private MapTile [,] mapTiles;

    public TileBase tileTypeToBase(MapTileType type) {
        switch(type) {
            case MapTileType.GRASS:
            default:
                return grassTileBase;
        }
    }

    public TileBase objectTypeToBase(MapObjectType type) {
        switch(type) {
            case MapObjectType.NONE:
                return null;
            case MapObjectType.GRASS:
            default:
                return envGrassTileBase;
        }
    }

    public int tileTypeToMalus(MapTileType type) {
        switch(type) {
            case MapTileType.GRASS:
            default:
                return 1;
        }
    }

    public int objectTypeToMalus(MapObjectType type) {
        switch(type) {
            case MapObjectType.BUILDING:
                return 10;
            case MapObjectType.NONE:
            case MapObjectType.GRASS:
            default:
                return 1;
        }
    }

    public MapTileType nameToTileType(string name) {
        switch(name) {
            case "Water":
                return MapTileType.WATER;
            case "Grass":
            default:
                return MapTileType.GRASS;
        }
    }

    public MapTileType nameToObjectType(string name) {
        switch(name) {
            case "Grass":
            default:
                return MapTileType.GRASS;
        }
    }

    private bool isWaterType(string tileName) {
        foreach(TileBase tileBase in waterTileBases) {
            if(tileBase.name == tileName) {
                return true;
            }
        }

        return false;
    }

    private bool isRoadType(string tileName) {
        foreach(TileBase tileBase in roadTileBases) {
            if(tileBase.name == tileName) {
                return true;
            }
        }

        return false;
    }

    private bool objectCanBePlacedOnWater(MapObjectType objectType) {
        switch(objectType) {
            default:
                return false;
        }
    }

    private bool objectCanBePlacedOnRoads(MapObjectType objectType) {
        switch(objectType) {
            default:
                return false;
        }
    }

    private bool objectCanBePlaced(string tileName, MapObjectType objectType) {
        if(isWaterType(tileName) && !objectCanBePlacedOnWater(objectType)) {
            return false;
        }

        if(isRoadType(tileName) && !objectCanBePlacedOnRoads(objectType)) {
            return false;
        }

        return true;
    }

    void Start()
    {
        mapTiles = new MapTile[mapWidth, mapHeight];

        for(int i = 0; i < mapWidth; ++i) {
            for(int j = 0; j< mapHeight; ++j) {
                mapTiles[i,j] = new MapTile();
                mapTiles[i,j].tileType = MapTileType.GRASS;
                mapTiles[i,j].tileTypeMovementMalus = tileTypeToMalus(mapTiles[i,j].tileType);
                if(Random.Range(0, 3) == 1) {
                    mapTiles[i,j].objectType = MapObjectType.GRASS;
                    mapTiles[i,j].objectTypeMovementMalus = objectTypeToMalus(mapTiles[i,j].objectType);
                } else {
                    mapTiles[i,j].objectType = MapObjectType.NONE;
                    mapTiles[i,j].objectTypeMovementMalus = objectTypeToMalus(mapTiles[i,j].objectType);
                }
            }
        }

		if(groundLayer) {
			Tilemap groundTileMap = groundLayer.GetComponent<Tilemap>();
            Tilemap firstTileMap = firstLayer.GetComponent<Tilemap>();
			for(int i = 0; i < mapWidth; ++i) {
				for(int j = 0; j< mapHeight; ++j) {
                    Vector3Int curPos = new Vector3Int(i, j);
                    if(!groundTileMap.HasTile(curPos)) {
					    groundTileMap.SetTile(curPos, tileTypeToBase(mapTiles[i,j].tileType));
                    } else {
                        RuleTile existingTile = groundTileMap.GetTile<RuleTile>(curPos);
                        
                        mapTiles[i,j].tileType = nameToTileType(existingTile.name);
                        mapTiles[i,j].tileTypeMovementMalus = tileTypeToMalus(mapTiles[i,j].tileType);
                    }

                    if(!firstTileMap.HasTile(curPos)) {
                        RuleTile existingTile = groundTileMap.GetTile<RuleTile>(curPos);
                        if(objectCanBePlaced(existingTile.name, mapTiles[i,j].objectType)) {
					        firstTileMap.SetTile(curPos, objectTypeToBase(mapTiles[i,j].objectType));
                        }
                    } else {
                        RuleTile existingTile = firstTileMap.GetTile<RuleTile>(curPos);
                        
                        mapTiles[i,j].tileType = nameToObjectType(existingTile.name);
                        mapTiles[i,j].tileTypeMovementMalus = objectTypeToMalus(mapTiles[i,j].objectType);
                    }
				}
			}
		}
    }

    void Update()
    {
        
    }
}
