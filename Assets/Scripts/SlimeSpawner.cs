using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : EnemySpawner
{
    public Loot loot;
    public GameObject[] waterObjects;

    new void Start()
    {
        base.Start();
        loot.EnableLoot();
    }

    private void Update() 
    {
        if (loot.items.Count == 0) 
        {
            DisableWater();
            canSpawn = false;
        }
    }

    private void DisableWater()
    {
        foreach (GameObject go in waterObjects)
        {
            go.SetActive(false);
        }
    }
}
