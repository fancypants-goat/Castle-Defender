using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public Resource resource;
    private WorkerManager workerManager;
    void Start() 
    {
        resource.amount = Random.Range(10,21);
        workerManager = FindObjectOfType<WorkerManager>();
    }

    void Update()
    {
        if (resource.amount <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnMouseDown() 
    {
        GameManager.Instance.workerPanel.SetActive(true);
        workerManager.target = gameObject;
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
    Wood,
    Stone,
}