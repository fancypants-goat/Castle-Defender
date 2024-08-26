using UnityEngine;

public class MainBuildingScript : MonoBehaviour, IBuilding
{
    public int buildingHealth;

    public int BuildingHealth
    {
        get { return buildingHealth; }
        set { buildingHealth = value; }
    }
    public bool CanPlaceBuilding(Vector3 gridPosition, BuildingManager buildingManager, GameObject cursor)
    {
        throw new System.NotImplementedException("CanPlaceBuilding should not be called for MainBuildingScript.");
    }
}
