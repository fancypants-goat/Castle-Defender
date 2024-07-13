using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBox;

    public List<GameObject> totalWorkers = new List<GameObject>();
    [Space]

    private WorkerManager workerManager;
    private BuildManager buildManager;
    private bool mouseDown,isDragging;
    private Vector3 initialMousePos;
    void Start()
    {
        workerManager = FindObjectOfType<WorkerManager>();
        buildManager = FindObjectOfType<BuildManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!buildManager.isBuilding)
        {
            HandleMouseInput();
            if (isDragging)
            {
                SetRect();
            }
            SelectWorkers();
        }
    }

    void HandleMouseInput()
    {
        // On left click note initial mouse position
        if (Input.GetMouseButtonDown(0))
        {
            initialMousePos = Input.mousePosition;
            mouseDown = true;
        }
        // check if player is dragging mouse to select
        if (mouseDown && Vector3.Distance(Input.mousePosition, initialMousePos) > 1 && !isDragging)
        {
            isDragging = true;
            selectionBox.gameObject.SetActive(true);
        }
        // stop selection
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(SelectWorkers());
        }
    }

    void SetRect()
    {
        // set size of selection rectangle
        float width = Input.mousePosition.x - initialMousePos.x;
        float height = Input.mousePosition.y - initialMousePos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width),Mathf.Abs(height));
        selectionBox.position = (Input.mousePosition + initialMousePos)/2;
    }

    IEnumerator SelectWorkers()
    {
        // Clear previously selected workers if not dragging
        if (!isDragging)
        {
            workerManager.selectedWorkers.Clear();
        }

        // Get the selection box corners
        Vector3[] corners = new Vector3[4];
        selectionBox.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        foreach (var worker in totalWorkers)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worker.transform.position);


            // this can also be done by:
            // adding a collider to the selectionbox and using OnTriggerEnter2D in Worker.cs to add the worker to the list
            // this is a lot faster
            if (screenPos.x >= minX && screenPos.x <= maxX && screenPos.y >= minY && screenPos.y <= maxY)
            {
                if (!workerManager.selectedWorkers.Contains(worker))
                {
                    workerManager.selectedWorkers.Add(worker);
                }
                else
                {
                    workerManager.selectedWorkers.Remove(worker);
                }
            }
        }
        yield return null;
        isDragging = false;
        mouseDown = false;
        selectionBox.gameObject.SetActive(false);
    }
}
