using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class ContextMenuScript : MonoBehaviour
{
    public GameObject contextMenuWindow;
    private Vector3 oldMouseViewPortPos;
    void Start()
    {
		contextMenuWindow.SetActive(false);
    }

    private void handleMouseDown(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseViewPortPos = Camera.main.ScreenToViewportPoint(new Vector3(mousePos.x, -mousePos.y, mousePos.z));

        switch(key) {
            case 0:
                contextMenuWindow.SetActive(false);
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

        switch(key) {
            case 1:
                if(oldMouseViewPortPos == mouseViewPortPos) {
					contextMenuWindow.SetActive(true);
					Vector3 newPos = new Vector3(mouseViewPortPos.x * Camera.main.pixelWidth, (mouseViewPortPos.y * Camera.main.pixelHeight) + Camera.main.pixelHeight, 0);
					Debug.Log(newPos);
					contextMenuWindow.GetComponent<UIDocument>().rootVisualElement.transform.position = newPos;
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
