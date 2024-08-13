using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffScript : MonoBehaviour, IBuilding
{
    public bool CanPlaceBuilding(Vector3 gridPosition, BuildingManager buildingManager, GameObject cursor)
    {
        if (!buildingManager.IsOnExpansion(cursor.transform.position) 
        && buildingManager.CheckNextToAnyExpansion(cursor.transform.position)
        && !buildingManager.BuildingsContains(gridPosition))
        {
            return true;
        }
        return false;
    }
}
