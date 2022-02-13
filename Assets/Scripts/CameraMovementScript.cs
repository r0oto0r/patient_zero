using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public MapScript MainMapScript;
    public Grid MainGrid;
    private Camera mainCam;
    private bool scroll = false;
    private Vector3 bottom;
    private Vector3 left;
    private Vector3 top;
    private Vector3 right;
    private Vector3 cellSize;
    public float MouseSensivity = 0.4f;
    public float KeyBoardSensivity = 0.1f;
    void Start() {
        mainCam = GetComponent<Camera>();

        bottom = MainGrid.CellToWorld(new Vector3Int(0, 0));
        left = MainGrid.CellToWorld(new Vector3Int(0, MainMapScript.MapHeight - 1));
        top = MainGrid.CellToWorld(new Vector3Int(MainMapScript.MapWidth - 1, MainMapScript.MapHeight - 1));
        right = MainGrid.CellToWorld(new Vector3Int(MainMapScript.MapWidth - 1, 0));

        cellSize = MainGrid.cellSize;
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
            transform.position = transform.position + (mousDirection * (mainCam.orthographicSize / 10) * MouseSensivity);
        } else {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 keyBoardDirection = new Vector3(horizontal, vertical);
            transform.position = transform.position + (keyBoardDirection * (mainCam.orthographicSize / 10) * KeyBoardSensivity);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.PageUp)) {
            mainCam.orthographicSize -= 1;
            if(mainCam.orthographicSize < 3) {
                mainCam.orthographicSize = 3;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.PageDown)) {
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
