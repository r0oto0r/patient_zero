using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameEngine : MonoBehaviour
{	
	public GameObject zombiePrefab;
	public GameObject groundLayer;
	public TileBase grassTileBase;

    // Start is called before the first frame update
    void Start()
    {
		groundLayer = GameObject.Find("Ground1");
		if(groundLayer) {
			Tilemap tileMap = groundLayer.GetComponent<Tilemap>();
Debug.Log(grassTileBase.name);
			for(int i = 0; i < 256; ++i) {
				for(int j = 0; j< 256; ++j) {
					
					tileMap.SetTile(new Vector3Int(i, j), grassTileBase);
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
