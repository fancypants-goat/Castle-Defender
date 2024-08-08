using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [SerializeField] private BuildingManager buildManager;
    [Space]

    [SerializeField] private GameObject worker;
    [SerializeField] private Transform kingdom;
    [Space]

    public GameObject target;
    public List<GameObject> selectedWorkers = new List<GameObject>();
    public List<GameObject> totalWorkers = new List<GameObject>();
    void Start()
    {

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
        if (totalWorkers.Count < GameManager.Instance.totalWorkers)
        {
            // instantiates works
            // which can later be selected and send to get resource
            GameObject _worker = Instantiate(worker);
            _worker.SetActive(false);
            totalWorkers.Add(_worker);
        }
    }
}
