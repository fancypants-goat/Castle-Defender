using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]
    public GameObject resourceTarget;
    public Vector3 kingdomTarget;
    [SerializeField] LayerMask kingdomLayer;
    [SerializeField] private GameObject kingdom;
    public float speed;
    private float minimumDistance = 0.5f;
    public bool reachedTarget ,shouldMove ,working;
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
            if (shouldMove)
            {
                StartCoroutine(Move());
            }
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

    void TargetCheck()
    {
        // checks if there is a target
        shouldMove = resourceTarget != null;
    }

    IEnumerator Move()
    {
        // choose destination of worker depending on position of worker
        // switches between kingdom and resource
        working = true;
        Vector3 Destination = reachedTarget ? kingdomTarget : resourceTarget.transform.position;
        transform.position = Vector2.MoveTowards(transform.position,Destination, speed * Time.deltaTime);

        // counts as target reached when within radius of minimum distance
        if (Vector2.Distance(Destination,transform.position) < minimumDistance)
        {
            if (!reachedTarget)
            {
                yield return new WaitForSeconds(1);
                resourceTarget.GetComponent<ResourceItem>().resource.amount -= 1;
            }
            else
            {     
                resourceManager.AddResource(new Resource (resourceTarget.GetComponent<ResourceItem>().resource.resourceType,1));
            }
            reachedTarget = !reachedTarget;
        }
        TargetCheck();
        working = false;
    }
    void Idle()
    {
        if (resourceTarget  ==  null)
        {
            if (Vector2.Distance(transform.position, kingdomTarget) > 2)
            {
                transform.position = Vector2.Lerp(transform.position,kingdomTarget, Time.deltaTime);
            }
        }
    }

    void CalculateKingdomTarget()
    {
        // Casts a ray in the direction of main castle
        Vector3 raycastDir = (kingdom.transform.position - transform.position).normalized;
        RaycastHit2D targetPosition = Physics2D.Raycast(transform.position,raycastDir,Mathf.Infinity, kingdomLayer);
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
