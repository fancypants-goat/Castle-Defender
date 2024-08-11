    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExpansionManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    [Space]

    [SerializeField] private GameObject cursor, emptySpace;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;
    [SerializeField] private RuleTile expansion;
    [SerializeField] private Transform kingdom;
    [SerializeField] private Tilemap expansionTilemap;
    [Space]

    public bool isBuildingExpansion;
    [SerializeField] private ModeManager buildMode;
    [Space]
    [SerializeField] private int StartPrice;
    [SerializeField] private TMP_Text costText;
    public int cost;
    [Space]
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

    public List<Vector3> GetEmptyNeighbourPositions (Vector3 position)
    {
        Vector3[] directions = 
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
    };

    List<Vector3> neighborPositions = new List<Vector3>();

    // Loop through the directions and check for neighbors
    foreach (var direction in directions)
    {
        if (!ExpansionsContains(position + direction))
        {
            neighborPositions.Add(position + direction);
        }
    }

    return neighborPositions;
    }
    
    private Vector3 mousePos;
    [SerializeField] private Vector3Int relativeMousePos;

    void Update() 
    { 
        // get the mouseposition relative to the world
        if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width 
         && Input.mousePosition.y  >= 0 && Input.mousePosition.y <= Screen.height)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }
        
        CalculateRelativeMousePos();

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
    public void Building()
    {
        // set the position of this object to mousePos
        transform.position = mousePos;

        resourceManager.SubtractResource(new Resource(ResourceType.Wood, cost));
        StartCoroutine(PlaceExpansion(relativeMousePos));

    }

    public void ExpansionSelect(Vector3 position)
    {
        foreach (Vector3 emptyPosition in GetEmptyNeighbourPositions(relativeMousePos))
        {
            Vector3 worldPosition = emptyPosition + kingdom.transform.position;

            // Assuming your emptySpace prefab has a collider attached, 
            // we use OverlapPoint or OverlapCircle to check for existing objects
            Collider2D existingObject = Physics2D.OverlapCircle(worldPosition, 0.1f);

            // If no collider is found at this position, instantiate the empty space
            if (existingObject == null)
            {
                Instantiate(emptySpace, worldPosition, Quaternion.identity, kingdom.GetChild(3));
            }
        }
    }
    IEnumerator PlaceExpansion(Vector3Int position)
    {
        yield return null;
        // creating a new expansion at the position of the cursor
        // this also sticks the expansion to a grid using Mathf.RoundToInt()
        expansionTilemap.SetTile(position,expansion);
        // adds relative mouse position to list
        Expansion expansionData = new(relativeMousePos);
        AddNewUsableSpaces(expansionData);
    }

    private void AddNewUsableSpaces (Expansion current)
    {
        expansions.Add(current);
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
        relativeMousePos = new Vector3Int (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0);
    }
}


[System.Serializable]
public class Expansion
{
    public Vector3 position;
    public Expansion(Vector3 position) {
        this.position = position;
    }
}
