using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomManager : MonoBehaviour
{   
    public float zoomSpeed;
    [SerializeField] private Camera kingdomCamera;
    private Camera mainCamera;
    private Vector3 dragOrigin;
    void Start()
    {
        mainCamera = Camera.main;  // Get the main camera
    }

    void Update()
    {
        // Get the scroll input from the mouse wheel
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the orthographic size based on scroll input
        mainCamera.orthographicSize -= scrollData * zoomSpeed;

        // Clamp the orthographic size to stay within min and max bounds
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, kingdomCamera.orthographicSize - 2, kingdomCamera.orthographicSize + 2);

        // Handle panning
        HandlePan();

        // Adjust the camera position based on the new orthographic size
        ClampCameraPosition();
    }

    void HandlePan()
    {
        // Only allow panning if the camera is zoomed in
        if (mainCamera.orthographicSize < kingdomCamera.orthographicSize + 2)
        {
            // When the right mouse button is pressed down
            if (Input.GetMouseButtonDown(1))
            {
                dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            // When the right mouse button is held down
            if (Input.GetMouseButton(1))
            {
                Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mainCamera.transform.position += difference;
            }
        }
    }
    private void ClampCameraPosition()
    {
        // Calculate the screen bounds of the kingdom camera plus margin
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float kingdomCameraHeight = (kingdomCamera.orthographicSize + 2) * 2;
        float kingdomCameraWidth = kingdomCameraHeight * kingdomCamera.aspect;
        
        float marginHeight = kingdomCameraHeight;
        float marginWidth = kingdomCameraWidth;

        // Calculate the min and max bounds for the camera position
        float minX = kingdomCamera.transform.position.x - marginWidth / 2 + cameraWidth / 2;
        float maxX = kingdomCamera.transform.position.x + marginWidth / 2 - cameraWidth / 2;
        float minY = kingdomCamera.transform.position.y - marginHeight / 2 + cameraHeight / 2;
        float maxY = kingdomCamera.transform.position.y + marginHeight / 2 - cameraHeight / 2;

        // Clamp the camera position within the bounds
        Vector3 clampedPosition = mainCamera.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        mainCamera.transform.position = clampedPosition;
    }
}
