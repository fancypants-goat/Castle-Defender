using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]
    public Vector3 target;
    public bool shouldMove;
    public bool selected;
    public List<GameObject> selectedWorkers = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TargetDecider();
    }

    void TargetDecider()
    {
        // sets target for selected workers
        foreach (var worker in selectedWorkers)
        {
            Worker workerMovement = worker.GetComponent<Worker>();
            workerMovement.target = target;
            workerMovement.shouldMove = shouldMove;
        }
    }
}
