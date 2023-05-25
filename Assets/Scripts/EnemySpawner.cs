using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime = 30f;
    public GameObject spawn;
    public int maxSpawns = 4;
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
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            foreach (Attributes attr in spawnedAttributes)
            {
                if (!attr.alive) spawnedAttributes.Remove(attr);
            }
            if (spawnedAttributes.Count < maxSpawns)
            {
                float angle = Random.Range(maxAngle * (-1f), maxAngle);
                Vector3 spawnPos = new Vector3();
                spawnPos.x = transform.position.x + (spawnDistance * Mathf.Cos(angle / (180f / Mathf.PI)));
                spawnPos.y = transform.position.y;
                spawnPos.z = transform.position.z + (spawnDistance * Mathf.Sin(angle / (180f / Mathf.PI)));
                GameObject go = Instantiate(spawn, spawnPos, Quaternion.identity);
                spawnedAttributes.Add(go.GetComponent<Attributes>());
            }
        }
    }
}
