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
    public bool TryGetResource(ResourceType key, out Resource value) {
        // check if the resources dct contains the resource: key
        if (resources.ContainsKey(key)) { value = resources[key]; return true; } // set value to the resource with key: key. return true

        // set value to null
        value = null;
        // return false
        return false;
    }
    public Resource GetResource(ResourceType key) {
        // check if the resources dict contains the resource: key
        if (resources.ContainsKey(key)) return resources[key]; // return the resource with key: key

        // else return a new resource with amount: 0
        return new Resource (key, 0);
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