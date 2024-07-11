using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // replace ResourceSO with actual name of resource so script
    private Dictionary<ResourceType, Resource> resources = new();
    public Dictionary<ResourceType, Resource> Resources
    {
        get
        {
            return resources;
        }
    }
    public void AddResource (Resource resource) {
        if (resources.ContainsKey(resource.resourceType)) resources[resource.resourceType].amount += resource.amount;
        else resources.Add(resource.resourceType, resource);
        
    }
    public void SubtractResource (Resource resource) {
        resources[resource.resourceType].amount -= resource.amount;
        if (resources[resource.resourceType].amount <= 0) resources.Remove(resource.resourceType);
    }
}