using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private ResourceManager resourceManager;
    private BuildingManager buildingManager;

    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Stats")]
    public float speed;
    public float health;
    private float attackCooldown;

    private GameObject enemyTarget;

    public Resource[] drops;


    private void Start() 
    {
        enemyTarget = GameObject.Find("MainBuilding");
        resourceManager = FindObjectOfType<ResourceManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    void Update()
    {
        SelectState();
        UpdateState();

        enemyTarget = GetTarget();

        attackCooldown -= Time.deltaTime;
    }
    
    void SelectState()
    {
        if (health <= 0)
        {
            currentState = EnemyState.Death;
            return;
        }
        if (Vector2.Distance(enemyTarget.transform.position,transform.position) < 1)
        {
            currentState = EnemyState.Attacking;
            return;
        }
        currentState = EnemyState.Walking;
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Walking:
                StartWalking(enemyTarget.transform.position);
                break;
            case EnemyState.Attacking:
                StartAttacking();
                break;
            case EnemyState.Death:
                StartDeath();
                break;
        }
    }
    private void StartWalking (Vector3 target) 
    {
        Vector3 delta = target - transform.position;

        delta.Normalize();

        transform.Translate(speed * Time.deltaTime * delta);

    }

    private void StartAttacking()
    {
        if (attackCooldown <= 0)
        {
            enemyTarget.GetComponent<IBuilding>().BuildingHealth -= 1;
            attackCooldown = 1;
        }
        if (enemyTarget.GetComponent<IBuilding>().BuildingHealth < 0)
        {
            Destroy(enemyTarget);
            currentState = EnemyState.Walking;
        }
    }
    private void StartDeath()
    {
        foreach (Resource drop in drops) 
        {
            resourceManager.AddResource(drop);
        }
        
        Destroy(gameObject);
    }
    GameObject GetTarget()
    {
        GameObject target = null;
        foreach (Building building in buildingManager.Buildings)
        {
            if (building.buildingObject != null 
            && Vector3.Distance(building.buildingObject.transform.position,transform.position) 
            < Vector3.Distance(enemyTarget.transform.position,transform.position))
            {
                target = building.buildingObject;
            }
        }
        return target;
    }


}
    public enum EnemyState
    {
        Walking,
        Attacking,
        Death
    }