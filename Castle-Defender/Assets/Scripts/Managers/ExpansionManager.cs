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

    [SerializeField] private GameObject emptySpace;
    [SerializeField] private Tile expansion;
    [SerializeField] private Transform kingdom;
    [SerializeField] private Tilemap expansionTilemap;
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

    void Start() 
    {
        ExpansionSelect(kingdom.position);
    }
    void Update() 
    { 
        CostCalculator();
    }
    
    public void Building(Vector3Int position)
    {
        resourceManager.SubtractResource(new Resource(ResourceType.Wood, cost));
        StartCoroutine(PlaceExpansion(position));
    }

    public void ExpansionSelect(Vector3 position)
    {
        foreach (Vector3 emptyPosition in GetEmptyNeighbourPositions(position))
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
        Expansion expansionData = new(position);
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
