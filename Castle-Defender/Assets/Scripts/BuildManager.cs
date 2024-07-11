using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private SpriteRenderer cursorSpriteRenderer;
    [SerializeField] private GameObject expansion;
    [SerializeField] private Transform kingdom;
    [SerializeField] private bool isBuilding;
    private float cooldown;
    public HashSet<Vector3> usedSpaces = new() {
        new(0,0,0),
    };
    public HashSet<Vector3> usableSpaces = new() {
        new(1,0,0),
        new(-1,0,0),
        new (0,1,0),
        new(0,-1,0)
    };
    private Vector3 mousePos;
    private Vector3 relativeMousePos;
    void Update() 
    {
        
        // checks if mouse is within screen
        if (Input.mousePosition.x <= Screen.width && Input.mousePosition.x >= 0 &&
            Input.mousePosition.y <= Screen.height && Input.mousePosition.y >= 0)
        { 
            // get the mouseposition relative to the world
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
        }
        // set the position of cursor
        cursor.transform.position = mousePos;
        CursorColorCheck();
        SnapCursorToGridPosition();
        // Building Function
        Building();
    }

    public void Build()
    {
        // Switches Build Mode
        isBuilding = !isBuilding;
    }
    private void Building()
    {
        if (isBuilding)
        {
            // set the position of this object to mousePos
            transform.position = mousePos;

            // activating this object
            cursor.SetActive(true);

            // removing the time between this frame and last frame from cooldown
            cooldown -= Time.deltaTime;

            // calculates the difference between mouse and main kingdom
            relativeMousePos = new Vector3 (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0);

            // if the left mouse button is pressed and cooldown is less then or equals to 0
            if(Input.GetMouseButton(0) && cooldown <= 0 && usableSpaces.Contains(relativeMousePos)&& !usedSpaces.Contains(relativeMousePos))
            {
                // creating a new expansion at the position of the cursor
                // this also sticks the expansion to a grid using Mathf.RoundToInt()
                Instantiate(expansion,kingdom.position + relativeMousePos,Quaternion.identity,kingdom);
                // adds relative mouse position to list
                AddNewUsableSpaces(relativeMousePos);
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
    private void AddNewUsableSpaces(Vector3 current)
    {
        usedSpaces.Add(current);
        // adds values to the list
        usableSpaces.Add(new Vector3 (current.x + 1,current.y,0));
        usableSpaces.Add(new Vector3 (current.x - 1,current.y,0));
        usableSpaces.Add(new Vector3 (current.x,current.y + 1,0));
        usableSpaces.Add(new Vector3 (current.x,current.y - 1,0));
        usableSpaces.Remove(current);
        if (usableSpaces.Contains(new Vector3(0,0,0)))
        {
            usableSpaces.Remove(new Vector3(0,0,0));
        }
    }

    void CursorColorCheck()
    {
        if (usableSpaces.Contains(relativeMousePos) && !usedSpaces.Contains(relativeMousePos))
        {
            cursorSpriteRenderer.color = Color.green;
        }
        else
        {
            cursorSpriteRenderer.color = Color.red;
        }
    }
    void SnapCursorToGridPosition() {
        cursor.transform.position = new Vector3 (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0);
    }
}
