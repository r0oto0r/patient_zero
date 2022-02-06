using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour
{
    public GameObject mainMapObject;
    public GameObject mainGridObject;
    private Camera mainCam;
    private MapScript mainMapScript;
    private Grid mainGrid;
    private bool scroll = false;
    public Vector3 bottom;
    public Vector3 left;
    public Vector3 top;
    public Vector3 right;
    public Vector3 cellSize;
    public float mouseSensivity = 0.4f;
    public float keyBoardSensivity = 0.1f;
    void Start() {
        mainCam = GetComponent<Camera>();
        mainMapScript = mainMapObject.GetComponent<MapScript>();
        mainGrid = mainGridObject.GetComponent<Grid>();

        bottom = mainGrid.CellToWorld(new Vector3Int(0, 0));
        left = mainGrid.CellToWorld(new Vector3Int(0, mainMapScript.mapHeight - 1));
        top = mainGrid.CellToWorld(new Vector3Int(mainMapScript.mapWidth - 1, mainMapScript.mapHeight - 1));
        right = mainGrid.CellToWorld(new Vector3Int(mainMapScript.mapWidth - 1, 0));

        cellSize = mainGrid.cellSize;
    }

    void Update() {
        if(Input.GetMouseButtonDown(1)) {
            scroll = true;
        }
        if(Input.GetMouseButtonUp(1)) {
            scroll = false;
        }

        if(scroll) {
            float translationX = Input.GetAxis("Mouse X");
            float translationY = Input.GetAxis("Mouse Y");
            Vector3 mousDirection = new Vector3(-translationX, -translationY);
            transform.position = transform.position + (mousDirection * mouseSensivity);
        } else {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 keyBoardDirection = new Vector3(horizontal, vertical);
            transform.position = transform.position + (keyBoardDirection * keyBoardSensivity);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0) {
            mainCam.orthographicSize -= 1;
            if(mainCam.orthographicSize < 3) {
                mainCam.orthographicSize = 3;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0) {
            mainCam.orthographicSize += 1;
            if(mainCam.orthographicSize > 10) {
                mainCam.orthographicSize = 10;
            }
        }

        if(transform.position.y < bottom.y) {
            transform.position = new Vector3(transform.position.x, bottom.y, transform.position.z);
        }

        if(transform.position.x < left.x - cellSize.x / 2) {
            transform.position = new Vector3(left.x - cellSize.x / 2, transform.position.y, transform.position.z);
        }

        if(transform.position.y > top.y + cellSize.y / 2) {
            transform.position = new Vector3(transform.position.x, top.y + cellSize.y / 2, transform.position.z);
        }

        if(transform.position.x > right.x + cellSize.x / 2) {
            transform.position = new Vector3(right.x + cellSize.x / 2, transform.position.y, transform.position.z);
        }
    }
}
