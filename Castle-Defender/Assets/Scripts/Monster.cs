using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public float health;

    public Vector3 positionOfTarget;

    public GameObject drop;

    private void MoveMonsterTowardsPosition (Vector3 target) {
        // calculate the distance between self and the target
        Vector3 delta = target - transform.position;
        // normalize delta
        delta.Normalize();

        // move object towards the target
        // speed * deltatime determines the amount that should be moved
        // * delta determines the direction this object should be moved in
        transform.Translate(speed * Time.deltaTime * delta);
    }

    public void KillMonster () {
        // destroy this gameObject
        Destroy(gameObject);

        // drop the items
        DropItems();
    }
    public void DropItems() {
        // check if drop is actually assigned
        if (drop == null) return;

        // create a new instance of the drop prefab at this position
        Instantiate(drop, transform.position, Quaternion.identity);
    }

    // TEMPORARY
    private void OnCollisionEnter2D(Collision2D other)
    {
        // check if the collided object contains an instance of class "Monster"
        // this just checks if the collided object is also a monster
        if (other.gameObject.TryGetComponent<Monster>(out _)) {
            // kill this monster
            KillMonster();
        }
    }


    void Update()
    {
        // move the monster
        MoveMonsterTowardsPosition(positionOfTarget);
    }
}
