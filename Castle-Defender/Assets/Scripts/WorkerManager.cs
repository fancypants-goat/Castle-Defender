using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [SerializeField] private BuildingManager buildManager;
    [Space]

    [SerializeField] private SelectionManager selectionManager;
    [Space]

    [SerializeField] private GameObject worker;
    [SerializeField] private Transform kingdom;
    [Space]

    public GameObject target;
    public List<GameObject> selectedWorkers = new List<GameObject>();
    void Start()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetDecider();
        WorkerSpawn();
    }

    void TargetDecider()
    {
        // sets target for selected workers
        foreach (var worker in selectedWorkers)
        {
            Worker workerMovement = worker.GetComponent<Worker>();
            workerMovement.resourceTarget = target;
        }
    }

    void WorkerSpawn()
    {
        if (selectionManager.totalWorkers.Count < buildManager.Buildings.Count - 3)
        {
            float y = -0.6f;
            foreach (Building expansion in buildManager.Buildings)
            {
                if (expansion.position.x == 0 && expansion.position.y < y)
                {
                    y = expansion.position.y - 0.6f;
                }
            }
            GameObject newWorker = Instantiate(worker,new Vector2(kingdom.position.x, kingdom.position.y + y),Quaternion.identity);
            selectionManager.totalWorkers.Add(newWorker);
        }
    }
}
