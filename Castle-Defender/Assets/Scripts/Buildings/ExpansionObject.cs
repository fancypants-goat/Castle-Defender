using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionObject : MonoBehaviour
{
    public float zoomSpeed;
    public float zoomVelocity;
    private GameObject kingdom;
    private ExpansionManager expansionManager;
    private ResourceManager resourceManager;
    private Camera KingdomCamera;

    new private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        KingdomCamera = GameObject.Find("KingdomCamera").GetComponent<Camera>();
        kingdom = GameManager.Instance.kingdom;
        expansionManager = FindObjectOfType<ExpansionManager>();
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    void Update()
    {
        if (!IsObjectFullyInView()) {
            zoomVelocity += 0.0008f;
        }
        else {
            zoomVelocity -= 0.0002f;
        }

        if (zoomVelocity < 0) {
            zoomVelocity = 0;
        }
        if (zoomVelocity > 0.05f) {
            zoomVelocity = 0.05f;
        }

        KingdomCamera.orthographicSize += zoomVelocity;
        Camera.main.orthographicSize = KingdomCamera.orthographicSize + 2;
    }

    void OnMouseDown() 
    {
        if (resourceManager.GetResource(ResourceType.Wood).amount < expansionManager.cost) return;
        Vector3Int position = Vector3Int.FloorToInt(transform.position-kingdom.transform.position);
        expansionManager.Building(position);
        expansionManager.ExpansionSelect(position);
        Destroy(gameObject);
    }

    bool IsExpansionPartiallyInView() {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(KingdomCamera);
        Bounds bounds = renderer.bounds;

        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
    bool IsObjectFullyInView()
    {
        if (renderer == null || KingdomCamera == null)
        {
            return false;
        }

        Vector3[] corners = new Vector3[8];
        Bounds bounds = renderer.bounds;

        corners[0] = bounds.min;
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[4] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[6] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[7] = bounds.max;

        foreach (var corner in corners)
        {
            Vector3 viewportPoint = KingdomCamera.WorldToViewportPoint(corner);
            if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1 || viewportPoint.z < 0)
            {
                return false;
            }
        }

        return true;
    }
}
