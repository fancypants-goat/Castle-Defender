    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExpansionManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [SerializeField] private GameObject cursor;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;
    [SerializeField] private GameObject expansion;
    [SerializeField] private Transform kingdom;
    [Space]

    public bool isBuildingExpansion;
    [SerializeField] private bool canBuildOnSelectedGridPosition;
    [SerializeField] private BuildMode buildMode;
    [Space]
    [SerializeField] private int StartPrice;
    [SerializeField] private TMP_Text costText;
    private int cost;
    [Space]
    private float cooldown;
    [SerializeField] public HashSet<Expansion> expansions = new() {
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
        return ExpansionsContains(position + Vector3.up) ||
            ExpansionsContains(position + Vector3.down) ||
            ExpansionsContains(position + Vector3.right) ||
            ExpansionsContains(position + Vector3.left);
    }

    
    [SerializeField] private Vector3 mousePos, relativeMousePos;
    void Update() 
    { 
        // get the mouseposition relative to the world
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width 
         && Input.mousePosition.y  >= 0 && Input.mousePosition.y <= Screen.height)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }
        
        if (isBuildingExpansion) {    
            // calculates the difference between mouse and main kingdom

            CalculateRelativeMousePos();

            CheckIfCanBuildOnGridPosition();
            CursorColorCheck();

            SnapCursorToGridPosition();
            // Building Function
            Building();
        }
        if (!buildMode.buildUI)
        {
            isBuildingExpansion = false;
        }
        CostCalculator();
    }
    public void BuildExpanion()
    {
        // Switches Build Mode
        isBuildingExpansion= !isBuildingExpansion;
        // (de)activate the cursor depending on isBuildingExpansion
        cursor.SetActive(isBuildingExpansion);
    }
    private void Building()
    {
        if (!isBuildingExpansion) return;

        // set the position of this object to mousePos
        transform.position = mousePos;

        // removing the time between this frame and last frame from cooldown
        cooldown -= Time.unscaledDeltaTime;

        // if the left mouse button is pressed and cooldown is less then or equals to 0
        if (Input.GetMouseButton(0) && cooldown <= 0 && canBuildOnSelectedGridPosition)
        {

            if (resourceManager.GetResource(ResourceType.Wood).amount < cost) return;

            resourceManager.SubtractResource(new Resource(ResourceType.Wood, cost));

            StartCoroutine(PlaceExpansion(kingdom.position + relativeMousePos));
        }
    }
    IEnumerator PlaceExpansion(Vector3 position)
    {
        yield return null;
        // creating a new expansion at the position of the cursor
        // this also sticks the expansion to a grid using Mathf.RoundToInt()
        Instantiate(expansion, position, Quaternion.identity, kingdom);
        // adds relative mouse position to list
        Expansion expansionData = new(relativeMousePos);
        AddNewUsableSpaces(expansionData);
        // resetting the cooldown
        cooldown = 0.2f;
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
        Vector3 gridPosition = relativeMousePos;
        bool expansionOnThisGridPosition = ExpansionsContains(gridPosition);
        canBuildOnSelectedGridPosition = !expansionOnThisGridPosition && CheckForNeighbourExpansions(gridPosition);

        cursor.SetActive(!expansionOnThisGridPosition);
    }
    void SnapCursorToGridPosition() {
        cursor.transform.position = kingdom.position + relativeMousePos;
        cursorSpriteRenderer.sprite = expansion.GetComponent<SpriteRenderer>().sprite;
        cursor.transform.localScale = expansion.transform.localScale;
    }

    void CostCalculator()
    {
        int priceAddition = 0;
        if (expansions.Count > 4)
        {
            priceAddition = (expansions.Count-4) * StartPrice;
        }
        // Seperately calculates cost
        cost = StartPrice + priceAddition; 
        costText.text = cost.ToString();
    }
    
    void CalculateRelativeMousePos()
    {
        relativeMousePos = new Vector3 (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0);
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