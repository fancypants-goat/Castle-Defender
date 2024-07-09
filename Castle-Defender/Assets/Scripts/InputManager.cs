using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject expansion;
    [SerializeField] private Transform kingdom;
    [SerializeField] private bool isBuilding;
    private float cooldown;
    void Update() 
    {
        // get the mouseposition relative to the world
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // set the position of cursor
        cursor.transform.position = mousePos;
        // check if the player is building
        if (isBuilding)
        {
            // set the position of this object to mousePos
            transform.position = mousePos;

            // activating this object
            cursor.SetActive(true);

            // removing the time between this frame and last frame from cooldown
            cooldown -= Time.deltaTime;

            // if the left mouse button is pressed and cooldown is less then or equals to 0
            if (Input.GetMouseButton(0) && cooldown <= 0)
            {
                // creating a new expansion at the position of the cursor
                // this also sticks the expansion to a grid using Mathf.RoundToInt()
                Instantiate(expansion,kingdom.position + new Vector3 (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0),Quaternion.identity,kingdom);
                // resetting the cooldown
                cooldown = 0.2f;
            }
        }
        else
        {
            // deactivating the cursor object
            cursor.SetActive(false);
        }
    }

    public void Build()
    {
        // updated big if statement to this
        // this just does isBuilding = not isBuilding
        // for example -> isBuilding is true
        // isBuilding = !true = false
        // this is way shorter and saves time
        isBuilding = !isBuilding;
    }
}
