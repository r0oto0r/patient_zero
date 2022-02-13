using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileDetailsScript : MonoBehaviour
{
    public GameObject tileDetailsWindow;
    public GameObject tileCursor;
	public GameObject tileIndicator;
    public Grid mainGrid;
    public Tilemap groundLayer;
    public MapScript mapScript;
    private ContextMenuScript contextMenuScript;
	private bool tileDetailsWindowOpen = false;
    float timer = 0.0f;

    void Start()
    {
        tileDetailsWindow.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        contextMenuScript = GetComponent<ContextMenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        VisualElement rootVisualElement = tileDetailsWindow.GetComponent<UIDocument>().rootVisualElement;

        if(tileDetailsWindowOpen) {
			timer += Time.deltaTime;
		}
	
		if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Input.mousePosition;
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
			Vector3Int cellPos = groundLayer.WorldToCell(mouseWorldPos);

            if(!contextMenuScript.MousePosInWindow(mousePos) && cellPos.x >= 0 && cellPos.y >= 0 && cellPos.x < mapScript.MapWidth && cellPos.y < mapScript.MapHeight && groundLayer.HasTile(cellPos)) {
                GameObject groundTile = mapScript.GetGroundTileAt(cellPos);
                RuleTile groundRuleTile = groundLayer.GetTile<RuleTile>(cellPos);

                Vector3 cellCenterPosition = mainGrid.GetCellCenterWorld(cellPos);
                Vector3 newTileCursorPos = new Vector3(cellCenterPosition.x, cellCenterPosition.y + tileCursor.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f, 0);

                tileCursor.transform.position = newTileCursorPos;
                tileCursor.SetActive(true);

                tileIndicator.transform.position = cellCenterPosition;
                tileIndicator.SetActive(true);

                rootVisualElement.style.display = DisplayStyle.Flex;
                tileDetailsWindowOpen = true;

                Label cellNameValueLabel = rootVisualElement.Q<Label>("TileNameValueLabel");
                Label cellPosValueLabel = rootVisualElement.Q<Label>("TilePosValueLabel");
				Label cellIsBlockedValueLabel = rootVisualElement.Q<Label>("TileIsBlockedValueLabel");
                Label cellTypeValueLabel = rootVisualElement.Q<Label>("TileTypeValueLabel");
				Label cellMalusValueLabel = rootVisualElement.Q<Label>("TileMalusValueLabel");
				Label cellIsRoadValueLabel = rootVisualElement.Q<Label>("TileIsRoadValueLabel");
                Label cellIsRiverValueLabel = rootVisualElement.Q<Label>("TileIsRiverValueLabel");
                Label cellNumObjectsValueLabel = rootVisualElement.Q<Label>("TileNumObjectsValueLabel");
                VisualElement tileImage = rootVisualElement.Q<VisualElement>("TileImage");

				GroundTileScript groundTileScript = groundTile.GetComponent<GroundTileScript>();

                cellNameValueLabel.text = groundRuleTile.name;
                cellPosValueLabel.text = $"{cellPos}";
				cellIsBlockedValueLabel.text = groundTileScript.isBlocked ? "true" : "false";
				cellTypeValueLabel.text = Enum.GetName(typeof(GroundTileType), groundTileScript.type);
				cellMalusValueLabel.text = $"{mapScript.GetMalusAt(cellPos)}";
				cellIsRoadValueLabel.text = groundTileScript.isRoad ? "true" : "false";
				cellIsRiverValueLabel.text = groundTileScript.isRiver ? "true" : "false";
                cellNumObjectsValueLabel.text = $"{mapScript.GetObjectCount(cellPos)}";
				tileImage.style.backgroundImage = new StyleBackground(groundRuleTile.m_DefaultSprite);

                timer = 0f;
            }
		} else {
			if(tileDetailsWindowOpen) {
				int seconds = (int) timer % 60;
				if(seconds > 3) {
					timer = 0f;
                    rootVisualElement.style.display = DisplayStyle.None;
					tileCursor.SetActive(false);
					tileIndicator.SetActive(false);
					tileDetailsWindowOpen = false;
				}
			}
		}
    }
}
