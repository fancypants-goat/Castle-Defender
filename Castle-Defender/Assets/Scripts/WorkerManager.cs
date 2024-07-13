using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [SerializeField] private BuildManager buildManager;
    [Space]

    [SerializeField] private SelectionManager selectionManager;
    [Space]

    [SerializeField] private GameObject worker;
    [SerializeField] private Transform kingdom;
    [Space]

    public Vector3 target;
    public bool shouldMove;
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
            workerMovement.shouldMove = shouldMove;
        }
    }

    void WorkerSpawn()
    {
        if (selectionManager.totalWorkers.Count < buildManager.expansions.Count)
        {
            GameObject newWorker = Instantiate(worker,new Vector2(kingdom.position.x, kingdom.position.y - 2f),Quaternion.identity);
            selectionManager.totalWorkers.Add(newWorker);
        }
    }
}
