using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class ContextMenuScript : MonoBehaviour
{
    public GameObject contextMenuWindow;
    private Vector3 oldMouseWorldPos;
    void Start()
    {
        contextMenuWindow.SetActive(false);
    }

    private void handleMouseDown(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        switch(key) {
            case 0:
                contextMenuWindow.SetActive(false);
                break;
            case 1:
                oldMouseWorldPos = mouseWorldPos;
                break;
            default:
                break;
        }
    }
    
    private void handleMouseUp(int key) {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        switch(key) {
            case 1:
                if(oldMouseWorldPos == mouseWorldPos) {
                    contextMenuWindow.transform.position = mouseWorldPos;
                    contextMenuWindow.SetActive(true);
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
