using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public float resources = 10f;

    public void AddResource (Drop drop) {
        resources += drop.amount;
    }
}