using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileDetailsScript : MonoBehaviour
{	
    public GameObject mainGridObject;
    public GameObject tileCursor;
	public GameObject tileIndicator;
    private Grid mainGrid;
    public GameObject tileDetailsWindow;
    private GameObject groundLayer;
    public GameObject map;
    private MapScript mapScript;
	private bool tileDetailsWindowOpen = false;
    float timer = 0.0f;

    void Start()
    {
        mapScript = map.GetComponent<MapScript>();
        mainGrid = mainGridObject.GetComponent<Grid>();
        groundLayer = GameObject.Find("Ground");
		tileDetailsWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tileDetailsWindowOpen) {
			timer += Time.deltaTime;
		}
	
		if(Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Input.mousePosition;
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
			Tilemap tileMap = groundLayer.GetComponent<Tilemap>();
			Vector3Int cellPos = tileMap.WorldToCell(mouseWorldPos);

            if(cellPos.x >= 0 && cellPos.y >= 0 && cellPos.x < mapScript.mapWidth && cellPos.y < mapScript.mapHeight) {
                RuleTile tileAtPos = tileMap.GetTile<RuleTile>(cellPos);
                Vector3 cellCenterPosition = mainGrid.GetCellCenterWorld(cellPos);
                Vector3 newTileCursorPos = new Vector3(cellCenterPosition.x, cellCenterPosition.y + tileCursor.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f, 0);
                
                tileCursor.transform.position = newTileCursorPos;
                tileCursor.SetActive(true);

                tileIndicator.transform.position = cellCenterPosition;
                tileIndicator.SetActive(true);

                tileDetailsWindow.SetActive(true);
                tileDetailsWindowOpen = true;

                VisualElement rootVisualElement = tileDetailsWindow.GetComponent<UIDocument>().rootVisualElement;
                Label cellNameValueLabel = rootVisualElement.Q<Label>("TileNameValueLabel");
                Label cellPosValueLabel = rootVisualElement.Q<Label>("TilePosValueLabel");
                VisualElement tileImage = rootVisualElement.Q<VisualElement>("TileImage");
                tileImage.style.backgroundImage = new StyleBackground(tileAtPos.m_DefaultSprite);

                cellPosValueLabel.text = $"{cellPos}";
                cellNameValueLabel.text = tileAtPos.name;

                timer = 0f;
            }
		} else {
			if(tileDetailsWindowOpen) {
				int seconds = (int) timer % 60;
				if(seconds > 3) {
					timer = 0f;
					tileDetailsWindow.SetActive(false);
					tileCursor.SetActive(false);
					tileIndicator.SetActive(false);
					tileDetailsWindowOpen = false;
				}
			}
		}
    }
}
