using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    public Resource resource;

    // stuff
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