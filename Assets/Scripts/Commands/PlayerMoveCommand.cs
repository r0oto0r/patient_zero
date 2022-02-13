using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMoveCommand : Command {
    private PlayerScript playerScript;
    private Tilemap unitTileMap;
    private Vector3Int prevPos;
    private Vector3Int newPos;
    private bool posSet;
    private LineRenderer lineRenderer;
    public PlayerMoveCommand(PlayerScript playerScript, GameObject targetLine, Tilemap unitTileMap, Vector3Int newPos) {
        this.playerScript = playerScript;
        this.unitTileMap = unitTileMap;
        this.newPos = newPos;
        this.prevPos = playerScript.prevPosition;
        lineRenderer = targetLine.GetComponent<LineRenderer>();

    }

    public string GetName() {
        return "Move here";
    }

    public string GetCommandText() {
        return $"Moving from {this.prevPos} to {this.newPos}";
    }

    private void drawQuadraticBezierCurve(Vector3Int point0, Vector3Int point1) {
        Vector3 wordPoint0 = this.unitTileMap.GetCellCenterWorld(point0);
        Vector3 wordPoint1 = this.unitTileMap.GetCellCenterWorld(point1);
        Vector3 centerPoint = Vector3.Lerp(wordPoint0, wordPoint1, 0.5f);

        this.lineRenderer.positionCount = this.lineRenderer.positionCount + 2;
        this.lineRenderer.SetPosition(this.lineRenderer.positionCount - 2, wordPoint0);
        this.lineRenderer.SetPosition(this.lineRenderer.positionCount - 1, wordPoint1);
    }

    public bool Execute() {
        this.playerScript.MoveToPos(this.newPos);
        this.drawQuadraticBezierCurve(this.playerScript.prevPosition, this.newPos);
        return true;
    }

    public bool Undo() {
        this.playerScript.MoveToPos(this.prevPos);
        this.lineRenderer.positionCount = this.lineRenderer.positionCount > 0 ? this.lineRenderer.positionCount - 2 : 0;
        return true;
    }
}
