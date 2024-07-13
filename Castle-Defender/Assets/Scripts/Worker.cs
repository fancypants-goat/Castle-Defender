using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]
    public Vector3 target;
    [SerializeField] private GameObject kingdom;
    public float speed;
    private float minimumDistance = 1;
    public bool reachedTarget;
    public bool shouldMove;
    public bool working;
    private WorkerManager workerManager;
    void Start()
    {
        target = transform.position;
        kingdom = GameObject.FindGameObjectWithTag("Kingdom");
        workerManager = kingdom.GetComponent<WorkerManager>();
        resourceManager = kingdom.GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove && !working)
        {
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

    IEnumerator Move()
    {
        // choose destination of worker depending on position of worker
        // switches between kingdom and resource
        working = true;
        Vector3 Destination = reachedTarget ? kingdom.transform.position : target;
        transform.position = Vector2.MoveTowards(transform.position,Destination, speed * Time.deltaTime);

        // counts as target reached when within radius of minimum distance
        if (Vector2.Distance(Destination,transform.position) < minimumDistance)
        {
            yield return new WaitForSeconds(1);
            reachedTarget = !reachedTarget;
        }
        // add resource when returns to kingdom
        if (Vector2.Distance(kingdom.transform.position,transform.position) < minimumDistance 
        && kingdom.transform.position == Destination)
        {
            resourceManager.AddResource(new Resource (ResourceType.Resource1,1));
        }
        working = false;
    }
}
