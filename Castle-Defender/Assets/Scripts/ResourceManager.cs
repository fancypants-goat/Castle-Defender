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
        // if the resource is present in the resources dictionary, add the amount to the resource
        if (resources.ContainsKey(resource.resourceType)) resources[resource.resourceType].amount += resource.amount;
        // else add the resource to the resources dict
        else resources.Add(resource.resourceType, resource);
        
    }
    public void SubtractResource (Resource resource) {
        // remove the amount from the resource
        resources[resource.resourceType].amount -= resource.amount;
        // if the resource amount is 0 or below, remove the resource from the resources dict
        if (resources[resource.resourceType].amount <= 0) resources.Remove(resource.resourceType);
    }
}