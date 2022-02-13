using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public Tilemap GroundLayer;
    public Tilemap[] ObjectLayers;
	public RuleTile GrassTileBase;
    public RuleTile ObjGrassTileBase;
    public int MapWidth = 256;
    public int MapHeight = 256;
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
        if(pos.x < MapWidth && pos.y < MapHeight) {
            return groundTiles[pos.x, pos.y];
        }

        return null;
    }

    public GameObject[] GetObjectsAt(Vector3Int pos) {
        if(pos.x < MapWidth && pos.y < MapHeight) {
            List<GameObject> gameObjects = new List<GameObject>();
            for(int i = 0; i < ObjectLayers.Length; ++i) {
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
        if(pos.x < MapWidth && pos.y < MapHeight) {
            for(int i = 0; i < ObjectLayers.Length; ++i) {
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

	public bool GetBlockedAt(Vector3Int pos) {
        GameObject groundTile = GetGroundTileAt(pos);
        GameObject[] objectTiles = GetObjectsAt(pos);

        if(!groundTile || groundTile.GetComponent<GroundTileScript>().isBlocked) {
            return true;
        }
        if(objectTiles != null && objectTiles.Length > 0) {
            foreach(GameObject objectTile in objectTiles) {
				if(objectTile.GetComponent<ObjectTileScript>().isBlocked) {
					return true;
				} 
            }
        }

        return false;
    }

    void Start()
    {
        groundTiles = new GameObject[MapWidth, MapHeight];
        objectTiles = new GameObject[ObjectLayers.Length, MapWidth, MapHeight];

        for(int i = 0; i < MapWidth; ++i) {
            for(int j = 0; j< MapHeight; ++j) {
                Vector3Int curPos = new Vector3Int(i, j);
                if(!GroundLayer.HasTile(curPos)) {
                    GroundLayer.SetTile(curPos, GrassTileBase);
                }
                groundTiles[i, j] = GroundLayer.GetInstantiatedObject(curPos);
                
                for(int k = 0; k < ObjectLayers.Length; ++k) {
                    if(!ObjectLayers[k].HasTile(curPos)) {
                        if(k == 0 && Random.Range(0, 3) == 1) {
                            if(objectCanBePlaced(ObjGrassTileBase.m_DefaultGameObject.GetComponent<ObjectTileScript>(), groundTiles[i, j].GetComponent<GroundTileScript>())) {
                                ObjectLayers[k].SetTile(curPos, ObjGrassTileBase);
                            }
                        }
                    }
                    objectTiles[k, i, j] = ObjectLayers[k].GetInstantiatedObject(curPos);
                }
            }
        }
    }

    void Update() {
    }
}
