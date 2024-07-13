using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public Resource resource;
    private Vector3 target;
    private WorkerManager workerManager;
    private SelectionManager selectionManager;

    void Start() 
    {
        workerManager = FindObjectOfType<WorkerManager>();
        selectionManager = FindObjectOfType<SelectionManager>();
    }
    void OnMouseDown() 
    {
        // set target for selected workers to itself
        target = transform.position;
        workerManager.target = target; 
        workerManager.shouldMove = true;
        // clear selected worker list 
        StartCoroutine(ClearList());
    }

    void OnMouseEnter() 
    {
        selectionManager.selectingResource = true;
    }
    void OnMouseExit() 
    {
        selectionManager.selectingResource = false;;
    }
    IEnumerator ClearList()
    {
        yield return null;
        workerManager.selectedWorkers.Clear();
        workerManager.shouldMove = false;
    }
}

[System.Serializable]
public class Resource
{
    public ResourceType resourceType;
    public int amount;

    public Resource (ResourceType resourceType, int amount) {
        this.resourceType = resourceType;
        this.amount = amount;
    }

}

public enum ResourceType
{
    Resource1,
    Resource2,
}