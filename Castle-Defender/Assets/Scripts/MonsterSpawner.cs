using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{

    public LevelBounds bounds;
    [Space]
    public GameObject monsterPrefab;
    public GameObject resourcePrefab;
    [Space]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<Tile> tiles = new List<Tile>();

    void Start() {
        Vector2 topLeft = new Vector2(bounds.left, bounds.top);
        Vector2 topRight = new Vector2(bounds.right, bounds.top);
        Vector2 bottomLeft = new(bounds.left, bounds.bottom);
        Vector2 bottomRight = new(bounds.right, bounds.bottom);

        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);

        // chooses the amount of enemy to be spawned
        int enemySpawnAmount = Random.Range(10,21); // Range numbers are placeholders for now
        // and runs the function that amount of time

        for (int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnMonster();
            SpawnResource();
        }
        SpawnTile();
    }

    private void SpawnMonster() {
        // make sure that monsterPrefab is assigned
        if (monsterPrefab == null) return;

        // get a random position in the level
        float x = Random.Range(bounds.left, bounds.right);
        float y = Random.Range(bounds.bottom, bounds.top);
        Vector3 monsterPosition = new(x, y);

        Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
    }

    private void SpawnResource()
    {
        // make sure that monsterPrefab is assigned
        if (resourcePrefab == null) return;

        // get a random position in the level
        float x = Random.Range(bounds.left, bounds.right);
        float y = Random.Range(bounds.bottom, bounds.top);
        Vector3 resourcePosition = new(x, y);

        Instantiate(resourcePrefab, resourcePosition, Quaternion.identity);
    }

    private void SpawnTile()
    {
        float length = Mathf.Abs(bounds.right) + Mathf.Abs(bounds.left);
        float height = Mathf.Abs(bounds.top) + Mathf.Abs(bounds.bottom);
        for (int x = bounds.left.ConvertTo<int>(); x < length; x++)
        {
            for (int y = bounds.bottom.ConvertTo<int>(); y < height; y++)
            {
                int tileNumber = Random.Range(1,100);
                if (tileNumber <= 8)
                {tilemap.SetTile(new Vector3Int(x,y,0),tiles[tileNumber]);}
                else
                {
                    tilemap.SetTile(new Vector3Int(x,y,0),tiles[0]);
                }
            }
        }
    }


    void Update() {
        
        Vector2 topLeft = new Vector2(bounds.left, bounds.top);
        Vector2 topRight = new Vector2(bounds.right, bounds.top);
        Vector2 bottomLeft = new(bounds.left, bounds.bottom);
        Vector2 bottomRight = new(bounds.right, bounds.bottom);

        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);
    }
}


//  custom vector4 for better understanding
[System.Serializable]
public class LevelBounds
{
    public float left;
    public float right;
    public float bottom;
    public float top;
}