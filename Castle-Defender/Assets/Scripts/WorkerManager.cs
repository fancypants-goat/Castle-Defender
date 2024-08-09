using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [SerializeField] private BuildingManager buildManager;
    [Space]

    [SerializeField] private GameObject worker, workerUI, workerMenuContent;
    [SerializeField] private Transform kingdom;
    [Space]

    public GameObject target;
    public HashSet<GameObject> selectedWorkers = new HashSet<GameObject>();
    public List<GameObject> totalWorkers = new List<GameObject>();
    public List<Button> workerButtons = new List<Button>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WorkerSpawn();
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

            // Corresponding button to select worker
            GameObject _workerUI = Instantiate(workerUI, workerMenuContent.transform);
            Button workerButton = _workerUI.GetComponent<Button>();
            workerButtons.Add(workerButton);

            // Saves the index of the button so that it can be 
            // later used when the respective worker is chosen
            int index = workerButtons.Count - 1;
            Image img = _workerUI.GetComponent<Image>();
            workerButton.onClick.AddListener(() => SelectWorker(index,img));
        }
    }

    // Select worker
    public void SelectWorker(int index, Image image)
    {
        // Adds/Removes worker and changes button color to show
        if (!selectedWorkers.Contains(totalWorkers[index]))
        {
            image.color = Color.green;
            selectedWorkers.Add(totalWorkers[index]);
        }
        else
        {
            image.color = Color.white;
            selectedWorkers.Remove(totalWorkers[index]);
        }
    }

    // Menu Manager
    public void WhenComplete()
    {
        // When Done is clicked runs Coroutine
        StartCoroutine(CompleteSelection());
    }
    public IEnumerator CompleteSelection()
    {
        GameManager.Instance.workerPanel.SetActive(false);
        yield return null;

        // Temporary list is created to clear selected workers 
        // to reset them to send another group of workers to a different resourcer
        HashSet<GameObject> temp = selectedWorkers;
        // each workers target is set with a slight gap
        foreach (var worker in temp)
        {
            worker.SetActive(true);
            Worker workerMovement = worker.GetComponent<Worker>();
            workerMovement.resourceTarget = target;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
