using UnityEngine;

public class ArcherTowerScript : MonoBehaviour, IBuilding
{
    [Header("Stats")]
    public int buildingHealth, range;
    public float attackCooldown;

    [Header("Enemy Calculation")]
    public GameObject projectile;
    private GameObject target;
    public Spawner spawner;
    private EnemyDetection enemyDetection = new EnemyDetection();
    public int BuildingHealth
    {
        get { return buildingHealth; }
        set { buildingHealth = value; }
    }
    public bool CanPlaceBuilding(Vector3 gridPosition, BuildingManager buildingManager, GameObject cursor)
    {
       if (buildingManager.IsOnExpansion(cursor.transform.position)
       && !buildingManager.CheckSurroundedbyExpansion(cursor.transform.position)
       && !buildingManager.BuildingsContains(gridPosition))
       {
        return true;
       }
       return false;
    }

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }
    void Update() 
    {
        target = enemyDetection.FindEnemy(spawner, gameObject);

        Shoot();
    }
    void Shoot()
    {
        if (target == null || attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        if ((target.transform.position - transform.position).sqrMagnitude < range * range)
        {
            Debug.Log("Shoot");
            attackCooldown = 1;
        }
    }
}
public class EnemyDetection
{
    public GameObject FindEnemy(Spawner spawner, GameObject tower)
    {
        GameObject target = spawner.enemies[0];
        foreach (GameObject enemy in spawner.enemies)
        {
            if ((enemy.transform.position - tower.transform.position).sqrMagnitude
            < (target.transform.position - tower.transform.position).sqrMagnitude)
            {
                target = enemy;
            }
        }
        return target;
    }
    
}
