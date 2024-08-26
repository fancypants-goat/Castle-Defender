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
        && buildingManager.CheckNextToSelectiveExpansion(cursor.transform.position,new Vector3[]{Vector3.down,Vector3.left,Vector3.right})
        && !buildingManager.BuildingsContains(gridPosition))
        {
            return true;
        }
        return false;
    }
}
