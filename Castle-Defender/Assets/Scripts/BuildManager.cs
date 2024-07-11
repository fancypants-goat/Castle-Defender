using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
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
    [SerializeField] private bool isBuilding;
    [SerializeField] private bool canBuildOnSelectedGridPosition;

    [Space]
    [SerializeField] private float StartPrice;
    [SerializeField] private float PriceMultiplier;
    [Space]
    private float cooldown;
    public HashSet<Expansion> expansions = new() {
        new Expansion(Vector3.zero),
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
        bool nextToExpansion = ExpansionsContains
        (
            position + Vector3.up,
            position + Vector3.down,
            position + Vector3.left,
            position + Vector3.right
        );

        return nextToExpansion;
    }

    
    private Vector3 mousePos;
    private Vector3 relativeMousePos;
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
            relativeMousePos = new Vector3 (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0);

            CheckIfCanBuildOnGridPosition();
            CursorColorCheck();

            SnapCursorToGridPosition();
            // Building Function
            Building();
        }
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
            int cost = Mathf.FloorToInt(StartPrice + (PriceMultiplier * expansions.Count));
            if (resourceManager.GetResource(ResourceType.Resource1).amount < cost) return;

            resourceManager.SubtractResource(new Resource(ResourceType.Resource1, cost));

            // creating a new expansion at the position of the cursor
            // this also sticks the expansion to a grid using Mathf.RoundToInt()
            Instantiate(expansion, kingdom.position + relativeMousePos, Quaternion.identity, kingdom);
            // adds relative mouse position to list
            Expansion expansionData = new(relativeMousePos);
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
        if (canBuildOnSelectedGridPosition && (StartPrice + (PriceMultiplier * expansions.Count)) <= resourceManager.GetResource(ResourceType.Resource1).amount)
        {
            cursorSpriteRenderer.color = Color.green;
        }
        else
        {
            cursorSpriteRenderer.color = Color.red;
        }
    }
    void CheckIfCanBuildOnGridPosition() {
        bool expansionOnThisGridPosition = ExpansionsContains(relativeMousePos);
        canBuildOnSelectedGridPosition = !expansionOnThisGridPosition && CheckForNeighbourExpansions(relativeMousePos);

        if (!ExpansionsContains(relativeMousePos)) {
            cursor.SetActive(true);
        }
        else {
            cursor.SetActive(false);
        }
    }
    void SnapCursorToGridPosition() {
        cursor.transform.position = kingdom.position + relativeMousePos;
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