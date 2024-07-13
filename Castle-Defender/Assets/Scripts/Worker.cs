using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]
    public Vector3 resourceTarget;
    public Vector3 kingdomTarget;
    [SerializeField] LayerMask kingdomLayer;
    [SerializeField] private GameObject kingdom;
    public float speed;
    private float minimumDistance = 1;
    public bool reachedTarget;
    public bool shouldMove;
    public bool working;
    private WorkerManager workerManager;
    void Start()
    {
        resourceTarget = transform.position;
        kingdom = GameObject.FindGameObjectWithTag("Kingdom");
        workerManager = kingdom.GetComponent<WorkerManager>();
        resourceManager = kingdom.GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        calculateKingdomTarget();
        ColorChange();
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

    void ColorChange()
    {
        if (workerManager.selectedWorkers.Contains(gameObject))
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    IEnumerator Move()
    {
        // choose destination of worker depending on position of worker
        // switches between kingdom and resource
        working = true;
        Vector3 Destination = reachedTarget ? kingdomTarget : resourceTarget;
        transform.position = Vector2.MoveTowards(transform.position,Destination, speed * Time.deltaTime);

        // counts as target reached when within radius of minimum distance
        if (Vector2.Distance(Destination,transform.position) < minimumDistance)
        {
            if (!reachedTarget)
            {yield return new WaitForSeconds(1);}
            reachedTarget = !reachedTarget;
        }
        // add resource when returns to kingdom
        if (Vector2.Distance(kingdomTarget,transform.position) < minimumDistance 
        && kingdomTarget == Destination)
        {
            resourceManager.AddResource(new Resource (ResourceType.Resource1,1));
        }
        working = false;
    }

    void calculateKingdomTarget()
    {
        Vector3 raycastDir = (kingdom.transform.position - transform.position).normalized;
        RaycastHit2D targetPosition = Physics2D.Raycast(transform.position,raycastDir,Mathf.Infinity, kingdomLayer);
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
