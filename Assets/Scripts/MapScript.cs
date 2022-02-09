using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public Tilemap groundLayer;
    public Tilemap[] objectLayers;
	public RuleTile grassTileBase;
    public RuleTile objGrassTileBase;
    public int mapWidth = 256;
    public int mapHeight = 256;
    private GameObject [,] groundTiles;
    private GameObject [,,] objectTiles;

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
            for(int i = 0; i < objectLayers.Length; ++i) {
                GameObject objectAtTile = objectTiles[i, pos.x, pos.y];
                if(objectAtTile) {
                    gameObjects.Add(objectAtTile);
                }
            }

            return gameObjects.ToArray();
        }

        return null;
    }

    public int GetObjectCount(Vector3Int pos) {
        int total = 0;
        if(pos.x < mapWidth && pos.y < mapHeight) {
            for(int i = 0; i < objectLayers.Length; ++i) {
                if(objectTiles[i, pos.x, pos.y]) {
                    total++;
                }
            }
        }

        return total;
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
               malus += objectTile.GetComponent<ObjectTileScript>().malus; 
            }
        }

        return malus;
    }

    void Start()
    {
        groundTiles = new GameObject[mapWidth, mapHeight];
        objectTiles = new GameObject[objectLayers.Length, mapWidth, mapHeight];

        for(int i = 0; i < mapWidth; ++i) {
            for(int j = 0; j< mapHeight; ++j) {
                Vector3Int curPos = new Vector3Int(i, j);
                if(!groundLayer.HasTile(curPos)) {
                    groundLayer.SetTile(curPos, grassTileBase);
                }
                groundTiles[i, j] = groundLayer.GetInstantiatedObject(curPos);
                
                for(int k = 0; k < objectLayers.Length; ++k) {
                    if(!objectLayers[k].HasTile(curPos)) {
                        if(k == 0 && Random.Range(0, 3) == 1) {
                            if(objectCanBePlaced(objGrassTileBase.m_DefaultGameObject.GetComponent<ObjectTileScript>(), groundTiles[i, j].GetComponent<GroundTileScript>())) {
                                objectLayers[k].SetTile(curPos, objGrassTileBase);
                            }
                        }
                    }
                    objectTiles[k, i, j] = objectLayers[k].GetInstantiatedObject(curPos);
                }
            }
        }
    }

    void Update() {
    }
}
