using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{
    // this is the amount that the camera zooms in/out per scroll
    public float amountPerZoom;

    new private Camera camera;

    void Start() {
        camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (camera.orthographicSize >= 2)
        {
            camera.orthographicSize -= amountPerZoom * Input.GetAxis("Mouse ScrollWheel");
        }
        else
        {
            camera.orthographicSize = 2;
        }
    }
}
