using UnityEngine;

public class HouseScript : MonoBehaviour, IBuilding
{
    public int buildingHealth;

    public int BuildingHealth
    {
        get { return buildingHealth; }
        set { buildingHealth = value; }
    }
    public bool CanPlaceBuilding(Vector3 gridPosition, BuildingManager buildingManager, GameObject cursor)
    {
        if (buildingManager.IsOnExpansion(cursor.transform.position) 
        && buildingManager.CheckSurroundedbyExpansion(cursor.transform.position)
        && !buildingManager.BuildingsContains(gridPosition))
        {
            return true;
        }
        return false;
    }
}
