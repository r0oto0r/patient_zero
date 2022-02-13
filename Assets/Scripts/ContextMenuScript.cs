using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class ContextMenuScript : MonoBehaviour
{
    public GameObject ContextMenuWindow;
    public Tilemap GroundLayer;
    public VisualTreeAsset ContextMenuItemPrefab;
    public CommandExecutorScript CommandExecutorScript;
    private Vector3 oldMouseViewPortPos;
    private PlayerScript playerScript;

    void Start()
    {
		ContextMenuWindow.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        playerScript = GetComponent<PlayerScript>();
    }

    public bool MousePosInWindow(Vector3 mousePos) {
        Vector3 mouseViewPortPos = Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x, -mousePos.y, mousePos.z));
        Vector3 mousePosOnScreen = new Vector3(mouseViewPortPos.x * Camera.main.pixelWidth, (mouseViewPortPos.y * Camera.main.pixelHeight) + Camera.main.pixelHeight, 0);
        VisualElement rootVisualElement = ContextMenuWindow.GetComponent<UIDocument>().rootVisualElement;
        VisualElement contextMenu = rootVisualElement.Q<VisualElement>("ContextMenu");
        return (     (mousePosOnScreen.x >= rootVisualElement.transform.position.x) &&
                (mousePosOnScreen.x <= (rootVisualElement.transform.position.x + contextMenu.resolvedStyle.width)) &&
                (mousePosOnScreen.y >= rootVisualElement.transform.position.y) &&
                (mousePosOnScreen.y <= (rootVisualElement.transform.position.y + contextMenu.resolvedStyle.height)));
    }

    private void handleMouseDown(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseViewPortPos = Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x, -mousePos.y, mousePos.z));
        VisualElement rootVisualElement = ContextMenuWindow.GetComponent<UIDocument>().rootVisualElement;

        switch(key) {
            case 0:
                if(rootVisualElement.style.display == DisplayStyle.Flex) {
                    if(!this.MousePosInWindow(mousePos)) {
                        rootVisualElement.style.display = DisplayStyle.None;
                    }
                }
                break;
            case 1:
                oldMouseViewPortPos = mouseViewPortPos;
                break;
            default:
                break;
        }
    }
    
    private void handleMouseUp(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseViewPortPos = Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x, -mousePos.y, mousePos.z));
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = GroundLayer.WorldToCell(mouseWorldPos);

        switch(key) {
            case 1:
                if(oldMouseViewPortPos == mouseViewPortPos) {
                    List<Command> commandList = playerScript.GetCommands(cellPos);
                    if(commandList.Count > 0) {
                        VisualElement rootVisualElement = ContextMenuWindow.GetComponent<UIDocument>().rootVisualElement;
                        Vector3 newPos = new Vector3(mouseViewPortPos.x * Camera.main.pixelWidth, (mouseViewPortPos.y * Camera.main.pixelHeight) + Camera.main.pixelHeight, 0);
                        rootVisualElement.style.display = DisplayStyle.Flex;
                        rootVisualElement.transform.position = newPos;

                        VisualElement contextMenu = rootVisualElement.Q<VisualElement>("ContextMenu");
                        contextMenu.Clear();
                        VisualElement contextMenuItem = ContextMenuItemPrefab.CloneTree();
                        Button contextMenuItemButton = contextMenuItem.Q<Button>("ContextMenuItemButton");

                        foreach(Command command in commandList) {
                            contextMenuItemButton.text = command.GetName();
                            contextMenuItemButton.clicked += () => {
                                CommandExecutorScript.AddCommand(command);
                                rootVisualElement.style.display = DisplayStyle.None;
                            };
                            contextMenu.Add(contextMenuItem);
                        }
                    }
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
        if(Input.GetMouseButtonDown(1)) {
            handleMouseDown(1);
        }
        if(Input.GetMouseButtonUp(1)) {
            handleMouseUp(1);
        }
    }
}
