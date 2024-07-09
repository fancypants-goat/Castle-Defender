using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float timeBetweenSpawns;

    [SerializeField]
    private float currentEvadedTime = 0f;

    [Space]
    public LevelBounds bounds;

    [Space]
    public GameObject monsterPrefab;

    private void SpawnMonster() {
        // make sure that monsterPrefab is assigned
        if (monsterPrefab == null) return;

        GameObject newMonster = Instantiate(monsterPrefab);

        // get a random position in the level
        float x = Random.Range(bounds.left, bounds.right);
        float y = Random.Range(bounds.bottom, bounds.top);
        Vector3 monsterPosition = new(x, y);

        newMonster.transform.position = monsterPosition;
    }


    void Update() {
        // add the time between this frame and last frame to currentEvadedTime
        currentEvadedTime += Time.deltaTime;

        // check if currentEvadedTime is more then timeBetweenSpawns
        // if true: spawn a new monster
        if (currentEvadedTime > timeBetweenSpawns) {
            currentEvadedTime = 0f;
            SpawnMonster();
        }
    }
}


//  custom vector4 for better understanding
[System.Serializable]
public class LevelBounds
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}