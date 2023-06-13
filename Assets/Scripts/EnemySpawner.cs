using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool canSpawn = true;
    public float spawnTime = 30f;
    public GameObject spawn;
    public int maxSpawns = 4;
    public int maxTotalSpawns = 100;
    private int totalSpawns = 0;
    public float spawnDistance = 5f;
    [Range(0f, 180f)]
    public float maxAngle = 90f;
    private List<Attributes> spawnedAttributes = new List<Attributes>();

    protected void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(spawnTime);
            if (totalSpawns == maxTotalSpawns) canSpawn = false;
            if (canSpawn)
            {
                List<Attributes> deadEnemies = new List<Attributes>();
                foreach (Attributes attr in spawnedAttributes)
                {
                    if (!attr.alive) deadEnemies.Add(attr);
                }
                foreach (Attributes attr in deadEnemies)
                {
                    spawnedAttributes.Remove(attr);
                }
                deadEnemies.Clear();
                if (spawnedAttributes.Count < maxSpawns)
                {
                    float angle = Random.Range(maxAngle * (-1f), maxAngle);
                    Vector3 spawnPos = new Vector3();
                    spawnPos.x = transform.position.x + (spawnDistance * Mathf.Sin((angle + transform.eulerAngles.y) / (180f / Mathf.PI))) ;
                    spawnPos.y = transform.position.y;
                    spawnPos.z = transform.position.z + (spawnDistance * Mathf.Cos((angle + transform.eulerAngles.y) / (180f / Mathf.PI)));
                    GameObject go = Instantiate(spawn, spawnPos, Quaternion.identity);
                    spawnedAttributes.Add(go.GetComponent<Attributes>());
                    totalSpawns++;
                }
            }
        }
    }
}
