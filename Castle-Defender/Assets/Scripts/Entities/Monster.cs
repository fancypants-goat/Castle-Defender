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

    public int difficulty = 0;

    [SerializeField] private float attackRadius;

    private GameObject enemyTarget;

    public Resource[] drops;

    void GetTarget()
    {
        foreach (Building building in buildingManager.Buildings)
        {
            if (building.buildingObject != null 
            && Vector3.Distance(building.buildingObject.transform.position,transform.position) 
            < Vector3.Distance(enemyTarget.transform.position,transform.position))
            {
                enemyTarget = building.buildingObject;
            }
        }
    }

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

        GetTarget();
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
        currentState = EnemyState.Idle;
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                StartIdle(enemyTarget.transform.position);
                break;
            case EnemyState.Attacking:
                StartAttacking();
                break;
            case EnemyState.Death:
                StartDeath();
                break;
        }
    }
    private void StartIdle (Vector3 target) 
    {
        Vector3 delta = target - transform.position;

        delta.Normalize();

        transform.Translate(speed * Time.deltaTime * delta);
    }

    private void StartAttacking()
    {

    }
    private void StartDeath()
    {
        foreach (Resource drop in drops) 
        {
            resourceManager.AddResource(drop);
        }
        
        Destroy(gameObject);
    }


}
    public enum EnemyState
    {
        Idle,
        Attacking,
        Death
    }