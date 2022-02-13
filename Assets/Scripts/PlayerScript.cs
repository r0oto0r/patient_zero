using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : BaseUnit
{
    public Vector3Int prevPosition = new Vector3Int(0, 0, 0);
    public Vector3Int position = new Vector3Int(0, 0, 0);
    public GameObject TargetLinePrefab;
    private GameObject targetLineInstance;
    public Tilemap unitTileMap;
    public TileBase playerTileBase;
    private Vector3Int oldMouseCellPos;
	public MapScript mapScript;

    void Start() {
        this.unitTileMap.SetTile(this.position, this.playerTileBase);
        this.targetLineInstance = GameObject.Instantiate(this.TargetLinePrefab, new Vector3(0,0,0), new Quaternion());
        this.targetLineInstance.GetComponent<LineRenderer>().positionCount = 0;
    }

    void Update() {

    }

    public void MoveToPos(Vector3Int position) {
        prevPosition = this.position;
        unitTileMap.SetTile(prevPosition, null);
        unitTileMap.SetTile(position, playerTileBase);
        this.position = position;
    }

    private PlayerMoveCommand move(Vector3Int cellPos) {
        return new PlayerMoveCommand(this, targetLineInstance, this.unitTileMap, cellPos);
    }

    public override List<Command> GetCommands(Vector3Int cellPos) {
        List<Command> commandList = new List<Command>();

        if(!mapScript.GetBlockedAt(cellPos)) {
            commandList.Add(this.move(cellPos));
        }

        return commandList;
    }
}
