using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]
    public GameObject resourceTarget;
    private Resource resource;
    private ResourceType resourceType;
    [Space]

    [SerializeField] LayerMask kingdomLayer;
    [SerializeField] private GameObject kingdom;
    public float speed;
    private float minimumDistance = 0.7f;
    public bool reachedTarget ,shouldMove ,working, carrying;
    private BuildingManager buildingManager;
    void Start()
    {
        kingdom = GameManager.Instance.kingdom;
        buildingManager = FindObjectOfType<BuildingManager>();
        resourceManager = kingdom.GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!working)
        {
            Idle();
            // move the worker
            if (shouldMove)
            {
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        // check if the worker has reached the target or the resourcetarget is not assigned
        Vector3 Destination = reachedTarget || resourceTarget == null ? kingdomTarget() : resourceTarget.transform.position;

        // check if the worker is in reach of the castle
        if (Vector2.Distance(transform.position, kingdomTarget()) <= minimumDistance && resourceTarget == null) {
            // stop the worker from moving
            gameObject.SetActive(false);
            yield break;
        }

        // choose destination of worker depending on position of worker
        // switches between kingdom and resource
        working = true;

        transform.position = Vector2.MoveTowards(transform.position, Destination, speed * Time.deltaTime);

        // resource identifier
        if (resourceTarget != null)
        {
            resource = resourceTarget.GetComponent<ResourceItem>().resource;
            resourceType = resource.resourceType;
        }
        // counts as target reached when within radius of minimum distance
        if (Vector2.Distance(Destination, transform.position) < minimumDistance)
        {
            if (!reachedTarget && resourceTarget != null)
            {
                yield return new WaitForSeconds(1);
                if (resourceTarget != null)
                {
                    carrying = true;
                    resource.amount--;
                }
            }
            if (Vector2.Distance(kingdomTarget(), transform.position) < minimumDistance && carrying)
            {     
                resourceManager.AddResource(new Resource (resourceType, 1));
                carrying = false;
            }
            reachedTarget = !reachedTarget;
        }
        yield return null;
        working = false;
    }
    void Idle()
    {
        // checking if resourceTarget is not assigned
        if (resourceTarget  ==  null)
        {
            // checking if the worker is more then 2 units away from the castle
            if (Vector2.Distance(transform.position, kingdomTarget()) > 2)
            {
                // setting the new position
                transform.position = Vector2.Lerp(transform.position, kingdomTarget(), Time.deltaTime);
            }
        }
        else
        {
            shouldMove = true;
        }
    }
    Vector3 kingdomTarget()
    {
        Vector3 ClosestDropoff = kingdom.transform.position;
        foreach (var building in buildingManager.DropOffs)
        {
            if (Vector2.Distance(building.transform.position,transform.position) < Vector2.Distance(ClosestDropoff,transform.position))
            {
                ClosestDropoff = building.transform.position;
            }
        }
        return ClosestDropoff;
    }
}
