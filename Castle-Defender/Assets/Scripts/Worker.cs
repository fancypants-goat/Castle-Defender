using System.Collections;
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

    public Vector3 kingdomTarget;
    [SerializeField] LayerMask kingdomLayer;
    [SerializeField] private GameObject kingdom;
    public float speed;
    private float minimumDistance = 0.5f;
    public bool reachedTarget ,shouldMove ,working, carrying;
    private WorkerManager workerManager;
    void Start()
    {
        kingdom = GameObject.FindGameObjectWithTag("Kingdom");
        workerManager = kingdom.GetComponent<WorkerManager>();
        resourceManager = kingdom.GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateKingdomTarget();
        ColorChange();
        if (!working)
        {
            Idle();
            // move the worker
            StartCoroutine(Move());
        }
    }

    void OnMouseDown() 
    {
        // select worker and add or remove it from list
        if (!workerManager.selectedWorkers.Contains(gameObject))
        {
            workerManager.selectedWorkers.Add(gameObject);
        }
        else
        {
            workerManager.selectedWorkers.Remove(gameObject);

        }
        reachedTarget = false;
    }

    void ColorChange()
    {
        GameObject selectionShow = gameObject.transform.GetChild(0).gameObject;
        if (workerManager.selectedWorkers.Contains(gameObject))
        {
            selectionShow.SetActive(true);
        }
        else
        {
            selectionShow.SetActive(false);
        }
    }

    IEnumerator Move()
    {
        // check if the worker has reached the target or the resourcetarget is not assigned
        Vector3 Destination = reachedTarget || resourceTarget == null ? kingdomTarget : resourceTarget.transform.position;

        // check if the worker is in reach of the castle
        if (Vector2.Distance(transform.position, kingdomTarget) <= 1 && resourceTarget == null) {
            // stop the worker from moving
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
        if (Vector2.Distance(Destination,transform.position) < minimumDistance)
        {
            if (!reachedTarget && resourceTarget != null)
            {
                yield return new WaitForSeconds(1);
                resource.amount -= 1;
                carrying = true;
            }
            else
            {     
                resourceManager.AddResource(new Resource (resourceType,1));
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
            if (Vector2.Distance(transform.position, kingdomTarget) > 2)
            {
                // setting the new position
                transform.position = Vector2.Lerp(transform.position, kingdomTarget, Time.deltaTime);
            }
        }
    }
    void CalculateKingdomTarget()
    {
        // Casts a ray in the direction of main castle
        Vector3 raycastDir = (kingdom.transform.position - transform.position).normalized;
        RaycastHit2D targetPosition = Physics2D.Raycast(transform.position, raycastDir, Mathf.Infinity, kingdomLayer);
        // if an expansion hits the ray 
        // makes worker drop off resource at that expansion
        if (targetPosition.collider != null)
        {
            kingdomTarget = targetPosition.transform.position;
        }
        else
        {
            kingdomTarget = kingdom.transform.position;
        }
        Debug.DrawLine(transform.position, kingdomTarget);
    }
}
