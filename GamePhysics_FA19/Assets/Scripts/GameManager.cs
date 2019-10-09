using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Asteroid Variables")]
    [SerializeField]
    private GameObject asteroidPrefab;
    [SerializeField]
    private int maxAsteroidCount;
    [SerializeField]
    private int currentAsteroidCount;

    // Asteroid Generation
    [Header("Asteroid Generation Variables")]
    [SerializeField]
    private float asteroidMinScale;
    [SerializeField]
    private float asteroidMaxScale;
    [SerializeField]
    private float asteroidMinSpeed;
    [SerializeField]
    private float asteroidMaxSpeed;

    [Header("Asteroid Timing")]
    [SerializeField]
    private float asteroidSpawnBuffer;
    private float asteroidSpawnTimer;

    // Edge of Map Variables
    public float edgeRight = 18.0f;
    public float edgeLeft = -18.0f;
    public float edgeTop = 10.3f;
    public float edgeBot = -10.3f;
    public float buffer = 0.1f;

    void Start()
    {

    }

    void Update()
    {
        CheckToSpawnAsteroid();
        // TO-DO: Once CheckEdgeOfView() is put into here, update for all asteroids
    }

    void CheckToSpawnAsteroid()
    {
        // Update asteroid counter
        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer > asteroidSpawnBuffer)
        {
            SpawnAsteroid();
            // Reset spawn timer
            asteroidSpawnTimer = 0.0f;
        }
    }

    void SpawnAsteroid()
    {
        // Spawn Asteroid at random location outside of map
        GameObject generatedAsteroid = Instantiate(GenerateAsteroid(), GenerateAsteroidSpawnPoint(), Quaternion.identity);
        // Generate force
        generatedAsteroid.GetComponent<Particle2D>().AddForceForward(Random.Range(asteroidMinSpeed, asteroidMaxSpeed));
    }

    GameObject GenerateAsteroid()
    {
        // Get asteroid prefab
        GameObject generatedAsteroid = asteroidPrefab;
        // Generate asteroid scale
        float asteroidScale = Random.Range(asteroidMinScale, asteroidMaxScale);
        generatedAsteroid.transform.localScale = new Vector3(asteroidScale, asteroidScale, asteroidScale);
        // Generate asteroid rotation
        float asteroidRotation = Random.Range(0, 360);
        generatedAsteroid.transform.eulerAngles = new Vector3(0.0f, 0.0f, asteroidRotation);
        // Return generated asteroid
        return generatedAsteroid;
    }

    Vector3 GenerateAsteroidSpawnPoint()
    {
        // TO-DO: Generate position outside of playspace
        return Vector3.zero;
    }
}
