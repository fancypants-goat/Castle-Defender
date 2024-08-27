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

    private GameObject enemyTarget, mainBuilding;

    public Resource[] drops;


    private void Start() 
    {
        mainBuilding = GameObject.Find("MainBuilding");
        enemyTarget = mainBuilding;
        resourceManager = FindObjectOfType<ResourceManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    void Update()
    {
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
        if ((enemyTarget.transform.position - transform.position).sqrMagnitude < 1)
        {
            currentState = EnemyState.Attacking;
            return;
        }
        if (enemyTarget != null)
        {
            currentState = EnemyState.Walking;
        }
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

        SelectState();
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
            DestroyBuilding(enemyTarget);
        }

        SelectState();
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
        GameObject target = mainBuilding;
        foreach (Building building in buildingManager.Buildings)
        {
            if (building.buildingObject != null 
            && (building.buildingObject.transform.position - transform.position).sqrMagnitude 
            < (enemyTarget.transform.position - transform.position).sqrMagnitude)
            {
                target = building.buildingObject;
            }
        }
        return target;
    }

    void DestroyBuilding(GameObject building)
    {
        Destroy(building);
        enemyTarget = mainBuilding;
        currentState = EnemyState.Walking;
    }

}
    public enum EnemyState
    {
        Walking,
        Attacking,
        Death
    }