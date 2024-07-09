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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = mousePos;
        if (isBuilding)
        {
            transform.position = mousePos;
            cursor.SetActive(true);
            cooldown -= Time.deltaTime;
            if (Input.GetMouseButton(0) && cooldown <= 0)
            {
                Instantiate(expansion,kingdom.position + new Vector3 (Mathf.RoundToInt(mousePos.x - kingdom.position.x),Mathf.RoundToInt(mousePos.y - kingdom.position.y),0),Quaternion.identity,kingdom);
                cooldown = 0.2f;
            }
        }
        else
        {
            cursor.SetActive(false);
        }
    }

    public void Build()
    {
        if (isBuilding)
        {
            isBuilding  = false; return;
        }
        isBuilding = true;
    }
}
