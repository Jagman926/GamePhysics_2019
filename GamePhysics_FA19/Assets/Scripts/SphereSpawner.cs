using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [Header("Sphere Spawner")]
    public GameObject sphereSpawnerCenter;
    [Range(-100,100)]
    public int minSpawnRange;
    [Range(-100, 100)]
    public int maxSpawnRange;

    [Header("Sphere")]
    public GameObject spherePrefab;


    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSphere();
        }
    }

    void SpawnSphere()
    {
        Instantiate(spherePrefab, new Vector3(Random.Range(minSpawnRange, maxSpawnRange), sphereSpawnerCenter.transform.position.y, Random.Range(minSpawnRange, maxSpawnRange)), Quaternion.identity, sphereSpawnerCenter.transform);
    }
}
