using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public Vector3 target;
    private WorkerManager workerManager;

    void Start() 
    {
        workerManager = FindObjectOfType<WorkerManager>();
    }
    void OnMouseDown() 
    {
        target = transform.position;
        workerManager.target = target; 
        workerManager.shouldMove = true;
        StartCoroutine(ClearList());
    }
    IEnumerator ClearList()
    {
        yield return null;
        workerManager.selectedWorkers.Clear();
        workerManager.shouldMove = false;
    }
}