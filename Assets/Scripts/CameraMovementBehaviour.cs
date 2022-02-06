using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour
{
    private Camera mainCam;
    private bool scroll = false;
    void Start() {
        mainCam = GetComponent<Camera>();
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
            transform.position = transform.position + mousDirection;
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
    }
}
