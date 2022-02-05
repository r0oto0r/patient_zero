using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameEngine : MonoBehaviour
{	
	public GameObject zombiePrefab;
	public GameObject groundLayer;
	public TileBase grassTileBase;
	public GameObject tileDetailsWindow;
	private bool tileDetailsWindowOpen = false;
float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
 		groundLayer = GameObject.Find("Ground");
		tileDetailsWindow.SetActive(false);
// 		if(groundLayer) {
// 			Tilemap tileMap = groundLayer.GetComponent<Tilemap>();
// Debug.Log(grassTileBase.name);
// 			for(int i = 0; i < 256; ++i) {
// 				for(int j = 0; j< 256; ++j) {
					
// 					tileMap.SetTile(new Vector3Int(i, j), grassTileBase);
// 				}
// 			}
// 		}
    }

    // Update is called once per frame
    void Update()
    {
		if(tileDetailsWindowOpen) {
			timer += Time.deltaTime;
		}
	
		if(Input.GetMouseButtonDown(0)) {
			timer = 0f;

			Vector3 mousePos = Input.mousePosition;
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
			Tilemap tileMap = groundLayer.GetComponent<Tilemap>();
			Vector3Int cellPos = tileMap.WorldToCell(mouseWorldPos);
			
			RuleTile tileAtPos = tileMap.GetTile<RuleTile>(cellPos);

			tileDetailsWindow.SetActive(true);
			tileDetailsWindowOpen = true;

			VisualElement rootVisualElement = tileDetailsWindow.GetComponent<UIDocument>().rootVisualElement;
			Label cellNameValueLabel = rootVisualElement.Q<Label>("TileNameValueLabel");
        	Label cellPosValueLabel = rootVisualElement.Q<Label>("TilePosValueLabel");
			VisualElement tileImage = rootVisualElement.Q<VisualElement>("TileImage");
			tileImage.style.backgroundImage = new StyleBackground(tileAtPos.m_DefaultSprite);

			cellPosValueLabel.text = $"{cellPos}";
			cellNameValueLabel.text = tileAtPos.name;
		} else {
			if(tileDetailsWindowOpen) {
				int seconds = (int) timer % 60;
				if(seconds > 3) {
					timer = 0f;
					tileDetailsWindow.SetActive(false);
					tileDetailsWindowOpen = false;
				}
			}
		}
    }
}
