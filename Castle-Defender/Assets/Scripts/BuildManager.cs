using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]
    [SerializeField] private GameObject cursor;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;
    [SerializeField] private GameObject expansion;
    [SerializeField] private Transform kingdom;
    public bool isBuilding;
    [SerializeField] private bool canBuildOnSelectedGridPosition;

    [Space]
    [SerializeField] private int StartPrice;
    [SerializeField] private TMP_Text costText;
    private int cost;
    [Space]
    private float cooldown;
    public HashSet<Expansion> expansions = new() {
        new Expansion(Vector3.zero)
    };
    public bool ExpansionsContains (Vector3 position) {
        foreach (Expansion expansion in expansions) {
            if (expansion.position == position) {
                return true;
            }
        }

        return false;
    }
    public bool ExpansionsContains (params Vector3[] positions) {
        foreach (Expansion expansion in expansions) {
            if (positions.Contains(expansion.position)) {
                return true;
            }
        }

        return false;
    }
    public bool CheckForNeighbourExpansions (Vector3 position) {
        return ExpansionsContains(position + Vector3.up/2) ||
            ExpansionsContains(position + Vector3.down/2) ||
            ExpansionsContains(position + Vector3.right/2) ||
            ExpansionsContains(position + Vector3.left/2);
    }

    
    [SerializeField] private Vector3 mousePos, relativeMousePos, closestExpansion;
    void Update() 
    { 
        // get the mouseposition relative to the world
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width 
         && Input.mousePosition.y  >= 0 && Input.mousePosition.y <= Screen.height)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }
        
        if (isBuilding) {    
            // calculates the difference between mouse and main kingdom
            closestExpansion = FindClosestExpansion(mousePos - kingdom.position);
            relativeMousePos = new Vector3 (
                Mathf.RoundToInt((mousePos.x - kingdom.position.x)*2) * 0.5f,
                Mathf.RoundToInt((mousePos.y - kingdom.position.y)*2) * 0.5f,
                0);
            CheckIfCanBuildOnGridPosition();
            CursorColorCheck();

            SnapCursorToGridPosition();
            // Building Function
            Building();
        }

        CostCalculator();
    }

    public void Build()
    {
        // Switches Build Mode
        isBuilding = !isBuilding;

        // (de)activate the cursor depending on isBuilding
        cursor.SetActive(isBuilding);
    }
    private void Building()
    {
        if (!isBuilding) return;

        // set the position of this object to mousePos
        transform.position = mousePos;

        // removing the time between this frame and last frame from cooldown
        cooldown -= Time.deltaTime;

        // if the left mouse button is pressed and cooldown is less then or equals to 0
        if (Input.GetMouseButton(0) && cooldown <= 0 && canBuildOnSelectedGridPosition)
        {

            if (resourceManager.GetResource(ResourceType.Wood).amount < cost) return;

            resourceManager.SubtractResource(new Resource(ResourceType.Wood, cost));

            // creating a new expansion at the position of the cursor
            // this also sticks the expansion to a grid using Mathf.RoundToInt()
            Instantiate(expansion, kingdom.position + closestExpansion + relativeMousePos, Quaternion.identity, kingdom);
            // adds relative mouse position to list
            Expansion expansionData = new(relativeMousePos + closestExpansion);
            AddNewUsableSpaces(expansionData);
            // resetting the cooldown
            cooldown = 0.2f;
        }
    }


    private void AddNewUsableSpaces (Expansion current)
    {
        expansions.Add(current);
    }

    void CursorColorCheck()
    {
        if (canBuildOnSelectedGridPosition && cost <= resourceManager.GetResource(ResourceType.Wood).amount)
        {
            cursorSpriteRenderer.color = Color.green;
        }
        else
        {
            cursorSpriteRenderer.color = Color.red;
        }
    }
    void CheckIfCanBuildOnGridPosition() {
        Vector3 gridPosition = relativeMousePos + closestExpansion;
        bool expansionOnThisGridPosition = ExpansionsContains();
        canBuildOnSelectedGridPosition = !expansionOnThisGridPosition && CheckForNeighbourExpansions(relativeMousePos);

        cursor.SetActive(!expansionOnThisGridPosition);
    }
    void SnapCursorToGridPosition() {
        cursor.transform.position = kingdom.position + relativeMousePos + closestExpansion;
    }

    Vector3 FindClosestExpansion(Vector3 position)
    {
        HashSet<Expansion> originalExpansions = new() {
        new Expansion(new Vector3(0.25f,0.25f,0)),
        new Expansion(new Vector3(-0.25f,0.25f,0)),
        new Expansion(new Vector3(0.25f,-0.25f,0)),
        new Expansion(new Vector3(-0.25f,-0.25f,0))
        };

        float minDistance = float.MaxValue;
        Vector3 closest = Vector3.zero;

        // checks which expansion is closest to the mouse position
        foreach (Expansion expansion in originalExpansions)
        {
            float distance = Vector3.Distance(position, expansion.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = expansion.position;
            }
        }

        return closest;
    }

    void CostCalculator()
    {
        int priceAddition = 0;
        if (expansions.Count() > 1)
        {
            priceAddition = expansions.Count * StartPrice;
        }
        // Seperately calculates cost
        cost = StartPrice + priceAddition; 
        costText.text = cost.ToString();
    }
}


[System.Serializable]
public class Expansion
{
    public Vector3 position;

    public Expansion() {

    }

    public Expansion (Vector3 position) {
        this.position = position;
    }
}