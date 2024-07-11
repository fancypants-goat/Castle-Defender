using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
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
        TargerDecider();
    }

    void TargerDecider()
    {
        foreach (var worker in selectedWorkers)
        {
            WorkerMovement workerMovement = worker.GetComponent<WorkerMovement>();
            workerMovement.target = target;
            workerMovement.shouldMove = shouldMove;
        }
    }
}
