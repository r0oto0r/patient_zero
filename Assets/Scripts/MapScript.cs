using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public GameObject groundLayer;
    public GameObject firstLayer;
    public GameObject secondLayer;
	public RuleTile grassTileBase;
    public RuleTile objGrassTileBase;
    public RuleTile[] roadTileBases;
    public RuleTile[] waterTileBases;
    public int mapWidth = 256;
    public int mapHeight = 256;
    public GroundTileScript [,] groundTiles;
    public ObjectTileScript [,] objectTiles;
    private Tilemap groundTileMap;
    private Tilemap firstTileMap;

    private bool objectCanBePlaced(ObjectTileScript objectTileScript, GroundTileScript groundTileScript) {
        if(!objectTileScript.canBePlacedOnWater && (groundTileScript.isRiver || groundTileScript.type == GroundTileType.WATER)) {
            return false;
        }

        if(!objectTileScript.canBePlacedOnRoad && groundTileScript.isRoad) {
            return false;
        }

        return true;
    }

    void Start()
    {
        groundTiles = new GroundTileScript[mapWidth, mapHeight];
        objectTiles = new ObjectTileScript[mapWidth, mapHeight];

        groundTileMap = groundLayer.GetComponent<Tilemap>();
        firstTileMap = firstLayer.GetComponent<Tilemap>();

        for(int i = 0; i < mapWidth; ++i) {
            for(int j = 0; j< mapHeight; ++j) {
                Vector3Int curPos = new Vector3Int(i, j);
                if(!groundTileMap.HasTile(curPos)) {
                    groundTileMap.SetTile(curPos, grassTileBase);
                }
                RuleTile existingGroundTile = groundTileMap.GetTile<RuleTile>(curPos);
                groundTiles[i,j] = existingGroundTile.m_DefaultGameObject.GetComponent<GroundTileScript>();

                if(Random.Range(0, 3) == 1) {
                    if(!firstTileMap.HasTile(curPos)) {
                        if(objectCanBePlaced(objGrassTileBase.m_DefaultGameObject.GetComponent<ObjectTileScript>(), groundTiles[i,j])) {
                            firstTileMap.SetTile(curPos, objGrassTileBase);
                        }
                    }
                    Tile existingObjectTile = groundTileMap.GetTile<Tile>(curPos);
                    objectTiles[i,j] = existingGroundTile.m_DefaultGameObject.GetComponent<ObjectTileScript>();
                }
            }
        }
    }

    void Update()
    {
        
    }
}
