using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [Header("Building Selection")]
    [SerializeField] public GameObject cursor;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;
    [SerializeField] public List<GameObject> buildingPrefabs = new List<GameObject>();
    private GameObject currentBuilding;
    private IBuilding buildingScript;
    [SerializeField] public List<Button> buildingButtons = new List<Button>();
    [SerializeField] private Transform kingdom;
    [Space]

    [Header("Building Placement")]
    public bool isBuildingBuilding;
    [SerializeField] private bool canBuildOnSelectedGridPosition;
    [SerializeField] private ModeManager buildMode;
    [Space]

    [Header("Cost Calculation")]
    [SerializeField] private int StartPrice;
    [SerializeField] private TMP_Text costText;
    private int cost;
    [Space]
    [SerializeField] private Tilemap expansionTileMap;
    private float cooldown;
    public HashSet<Building> Buildings = new() {
        new Building(new Vector3(0.25f,0.25f,0)),
        new Building(new Vector3(-0.25f,0.25f,0)),
        new Building(new Vector3(0.25f,-0.25f,0)),
        new Building(new Vector3(-0.25f,-0.25f,0))
    };
    public bool BuildingsContains (Vector3 position) {
        foreach (Building Building in Buildings) {
            if (Building.position == position) {
                return true;
            }
        }

        return false;
    }
    public bool BuildingsContains (params Vector3[] positions) {
        foreach (Building Building in Buildings) {
            if (positions.Contains(Building.position)) {
                return true;
            }
        }

        return false;
    }
    public bool IsOnExpansion(Vector3 position)
    {
        Vector3Int cellPosition = expansionTileMap.WorldToCell(position);
        return expansionTileMap.HasTile(cellPosition);
    }
    public bool CheckNextToAnyExpansion(Vector3 position)
    {

        return IsOnExpansion(position + Vector3.up/2)
            || IsOnExpansion(position + Vector3.down/2)
            || IsOnExpansion(position + Vector3.right/2)
            || IsOnExpansion(position + Vector3.left/2);
    }
    public bool CheckNextToSelectiveExpansion(Vector3 position, Vector3[] selectedPositions)
    {
        foreach (var current in selectedPositions)
        {
            if (IsOnExpansion(position + current/2))
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    private Vector3 mousePos, relativeMousePos, closestPoint;

    [Header("Extras")]
    public List<GameObject> DropOffs = new List<GameObject>();
    public GameObject buildingButtonPrefab;
    void Start() 
    {
        // sets each button to its building
        for (int i = 0; i < buildingButtons.Count; i++)
        {
            int index = i;
            buildingButtons[i].onClick.AddListener(() => SelectBuilding(index));
        }
    }
    void Update() 
    { 
        // get the mouseposition relative to the world
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width 
         && Input.mousePosition.y  >= 0 && Input.mousePosition.y <= Screen.height)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }
        
        if (isBuildingBuilding) {    
            // calculates the difference between mouse and main kingdom
            closestPoint = FindClosestBuilding(mousePos - kingdom.position);

            CalculateRelativeMousePos();
            CursorColorCheck();

            SnapCursorToGridPosition();
            // Building Function
            Building();
        }
        else
        {
            cursor.SetActive(false);
        }
            
        if (!buildMode.buildUI)
        {
            isBuildingBuilding = false;
        }
        CostCalculator();
    }
    public void SelectBuilding(int index)
    {
        if (currentBuilding == buildingPrefabs[index] && isBuildingBuilding)
        {
            isBuildingBuilding = false;
        }
        else
        {
            isBuildingBuilding = true;
        }
        // (de)activate the cursor depending on isBuildingBuilding
        cursor.SetActive(isBuildingBuilding);

        // sets building and cursor
        currentBuilding = buildingPrefabs[index];
        buildingScript = currentBuilding.GetComponent<IBuilding>();
        cursorSpriteRenderer.sprite = currentBuilding.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        cursor.transform.localScale = currentBuilding.transform.localScale;
    }
    private void Building()
    {
        if (!isBuildingBuilding) return;

        // set the position of this object to mousePos
        transform.position = mousePos;

        // removing the time between this frame and last frame from cooldown
        cooldown -= Time.unscaledDeltaTime;

        // if the left mouse button is pressed and cooldown is less then or equals to 0
        if (Input.GetMouseButton(0) && cooldown <= 0 && buildingScript.CanPlaceBuilding(relativeMousePos + closestPoint, this, cursor))
        {

            if (resourceManager.GetResource(ResourceType.Wood).amount < cost) return;

            resourceManager.SubtractResource(new Resource(ResourceType.Wood, cost));
            cooldown = 0.2f;

            StartCoroutine(PlaceBuilding(kingdom.position + closestPoint + relativeMousePos));
        }
    }
    IEnumerator PlaceBuilding(Vector3 position)
    {
        yield return null;
        // creating a new Building at the position of the cursor
        // this also sticks the Building to a grid using Mathf.RoundToInt()
        GameObject specificBuilding = Instantiate(currentBuilding, position, Quaternion.identity, kingdom.transform.GetChild(2));
        // adds relative mouse position to list
        Building BuildingData = new(relativeMousePos + closestPoint, specificBuilding);
        AddNewUsableSpaces(BuildingData);
        // resetting the cooldown
        // add to dropoff list
        if (currentBuilding == buildingPrefabs[0])
        {
            DropOffs.Add(specificBuilding);
            GameManager.Instance.totalWorkers += 1;
        }
    }

    private void AddNewUsableSpaces (Building current)
    {
        Buildings.Add(current);
    }

    void CursorColorCheck()
    {
        if (buildingScript.CanPlaceBuilding(relativeMousePos + closestPoint, this, cursor) && cost <= resourceManager.GetResource(ResourceType.Wood).amount)
        {
            cursorSpriteRenderer.color = Color.green;
        }
        else
        {
            cursorSpriteRenderer.color = Color.red;
        }
    }
    void SnapCursorToGridPosition() {
        cursor.transform.position = kingdom.position + relativeMousePos + closestPoint;
    }

    Vector3 FindClosestBuilding(Vector3 position)
    {
        HashSet<Building> originalBuildings = new() {
        new Building(new Vector3(0.25f,0.25f,0)),
        new Building(new Vector3(-0.25f,0.25f,0)),
        new Building(new Vector3(0.25f,-0.25f,0)),
        new Building(new Vector3(-0.25f,-0.25f,0))
        };

        float minDistance = float.MaxValue;
        Vector3 closest = Vector3.zero;

        // checks which Building is closest to the mouse position
        foreach (Building Building in originalBuildings)
        {
            float distance = Vector3.Distance(position, Building.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = Building.position;
            }
        }

        return closest;
    }

    void CostCalculator()
    {
        int priceAddition = 0;
        if (Buildings.Count > 4)
        {
            priceAddition = (Buildings.Count-4) * StartPrice;
        }
        // Seperately calculates cost
        cost = StartPrice + priceAddition; 
        costText.text = cost.ToString();
    }
    
    void CalculateRelativeMousePos()
    {
        float xDiff = mousePos.x - kingdom.position.x;
        float yDiff = mousePos.y - kingdom.position.y;

        float roundedX;
        float roundedY;

        if (xDiff >= 0)
        {
            roundedX = Mathf.Floor(xDiff * 2) * 0.5f;
        }
        else
        {
            roundedX = Mathf.Ceil(xDiff * 2) * 0.5f;
        }

        if (yDiff >= 0)
        {
            roundedY = Mathf.Floor(yDiff * 2) * 0.5f;
        }
        else
        {
            roundedY = Mathf.Ceil(yDiff * 2) * 0.5f;
        }

        relativeMousePos = new Vector3(roundedX, roundedY, 0);

    }

}

public interface IBuilding
{
    bool CanPlaceBuilding(Vector3 gridPosition, BuildingManager buildingManager, GameObject cursor);
}


[System.Serializable]
public class Building
{
    public Vector3 position;
    public GameObject buildingObject;
    public Building(Vector3 position, GameObject building) {
        this.position = position;
        buildingObject = building;
    }

    public Building (Vector3 position) {
        this.position = position;
    }
}