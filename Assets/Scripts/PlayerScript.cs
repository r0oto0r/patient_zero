using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public Vector3Int position = new Vector3Int(0, 0, 0);
    public Tilemap unitTileMap;
    public TileBase playerTileBase;
    private Vector3Int oldMouseCellPos;
	public MapScript mapScript;

    void Start()
    {
        unitTileMap.SetTile(this.position, playerTileBase);
    }

        private void handleMouseDown(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3Int mouseCellPos = unitTileMap.WorldToCell(mouseWorldPos);

        switch(key) {
            case 0:
                oldMouseCellPos = mouseCellPos;
                break;
            default:
                break;
        }
    }
    
    private void handleMouseUp(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3Int mouseCellPos = unitTileMap.WorldToCell(mouseWorldPos);

        switch(key) {
            case 0:
                if(oldMouseCellPos == mouseCellPos) {
                    moveToPos(unitTileMap.WorldToCell(mouseWorldPos));
                }
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            handleMouseDown(0);
        }
        if(Input.GetMouseButtonUp(0)) {
            handleMouseUp(0);
        }
    }
    public void moveToPos(Vector3Int position) {
		if(!mapScript.GetBlockedAt(position)) {
			unitTileMap.SetTile(this.position, null);
			unitTileMap.SetTile(position, playerTileBase);
			this.position = position;
		}
    }
}
