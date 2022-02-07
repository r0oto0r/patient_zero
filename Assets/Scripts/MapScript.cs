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
    public int numLayers = 2;
    public GameObject [,] groundTiles;
    public GameObject [,,] objectTiles;
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

    public GameObject GetGroundTileAt(Vector3Int pos) {
        if(pos.x < mapWidth && pos.y < mapHeight) {
            return groundTiles[pos.x, pos.y];
        }

        return null;
    }

    public GameObject[] GetObjectsAt(Vector3Int pos) {
        if(pos.x < mapWidth && pos.y < mapHeight) {
            List<GameObject> gameObjects = new List<GameObject>();
            for(int i = 0; i < numLayers; ++i) {
                gameObjects.Add(objectTiles[i, pos.x, pos.y]);
            }

            return gameObjects.ToArray();
        }

        return null;
    }

    public int GetMalusAt(Vector3Int pos) {
        GameObject groundTile = GetGroundTileAt(pos);
        GameObject[] objectTiles = GetObjectsAt(pos);
        int malus = 0;
        if(groundTile) {
            malus += groundTile.GetComponent<GroundTileScript>().malus;
        }
        if(objectTiles != null && objectTiles.Length > 0) {
            foreach(GameObject objectTile in objectTiles) {
               malus += objectTile.GetComponent<GroundTileScript>().malus; 
            }
        }

        return malus;
    }

    void Start()
    {
        groundTiles = new GameObject[mapWidth, mapHeight];
        objectTiles = new GameObject[numLayers, mapWidth, mapHeight];

        groundTileMap = groundLayer.GetComponent<Tilemap>();
        firstTileMap = firstLayer.GetComponent<Tilemap>();

        for(int i = 0; i < mapWidth; ++i) {
            for(int j = 0; j< mapHeight; ++j) {
                Vector3Int curPos = new Vector3Int(i, j);
                if(!groundTileMap.HasTile(curPos)) {
                    groundTileMap.SetTile(curPos, grassTileBase);
                }
                groundTiles[i, j] = groundTileMap.GetInstantiatedObject(curPos);

                if(Random.Range(0, 3) == 1) {
                    if(!firstTileMap.HasTile(curPos)) {
                        if(objectCanBePlaced(objGrassTileBase.m_DefaultGameObject.GetComponent<ObjectTileScript>(), groundTiles[i, j].GetComponent<GroundTileScript>())) {
                            firstTileMap.SetTile(curPos, objGrassTileBase);
                        }
                    }
                    objectTiles[0, i, j] = firstTileMap.GetInstantiatedObject(curPos);
                }
            }
        }
    }

    void Update() {
    }
}
