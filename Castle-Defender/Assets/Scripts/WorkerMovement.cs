using Unity.VisualScripting;
using UnityEngine;

public class WorkerMovement : MonoBehaviour
{
    public Vector3 target;
    [SerializeField] private GameObject kingdom;
    public float speed;
    private float minimumDistance = 1;
    public bool selected;
    public bool reachedTarget;
    public bool shouldMove;
    private SpriteRenderer spriteRenderer;
    private WorkerManager workerManager;
    void Start()
    {
        target = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        kingdom = GameObject.FindGameObjectWithTag("Kingdom");
        workerManager = kingdom.GetComponent<WorkerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ColorChange();
        if (shouldMove)
        {
            Move();
        }
    }

    void OnMouseDown() 
    {
        selected = !selected;
        if (selected)
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
        spriteRenderer.color = selected ? Color.green : Color.white;
    }

    void Move()
    {
        Vector3 Destination = reachedTarget ? kingdom.transform.position : target;
        transform.position = Vector2.MoveTowards(transform.position,Destination, speed * Time.deltaTime);
      
        if (Vector2.Distance(Destination,transform.position) < minimumDistance)
        {
            reachedTarget = !reachedTarget;
        }   
    }
}
