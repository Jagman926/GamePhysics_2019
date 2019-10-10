using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Asteroid Variables")]
    [SerializeField]
    private GameObject asteroidPrefab;
    [SerializeField]
    private int maxAsteroidCount;

    // List of asteroids
    private List<GameObject> asteroidsList;

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

    [Header("Game Over Stuff")]
    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField]
    private GameObject player;

    void Start()
    {
        asteroidsList = new List<GameObject>();
        gameOverCanvas.enabled = false;
    }

    void Update()
    {
        CheckToSpawnAsteroid();
        CheckAsteroidWrap();
    }

    public void CheckEdgeOfView(ref Particle2D p2D)
    {
        // If moves off right side of view, move to left side
        if (p2D.position.x > edgeRight + buffer)
            p2D.position = new Vector2(edgeLeft - buffer, transform.position.y);
        // If moves off left side of view, move to right side
        else if (p2D.position.x < edgeLeft - buffer)
            p2D.position = new Vector2(edgeRight + buffer, transform.position.y);
        // If moves off top of view, move to bottom side
        else if (p2D.position.y > edgeTop + buffer)
            p2D.position = new Vector2(transform.position.x, edgeBot - buffer);
        // If moves off bottom of view, move to top side
        else if (p2D.position.y < edgeBot - buffer)
            p2D.position = new Vector2(transform.position.x, edgeTop + buffer);
    }

    void CheckAsteroidWrap()
    {
        foreach (GameObject asteroid in asteroidsList)
        {
            Particle2D asteroidP2D = asteroid.GetComponent<Particle2D>();
            CheckEdgeOfView(ref asteroidP2D);
        }
    }

    void CheckToSpawnAsteroid()
    {
        // Update asteroid counter
        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer > asteroidSpawnBuffer && asteroidsList.Count < maxAsteroidCount)
        {
            SpawnAsteroid();
            // Reset spawn timer
            asteroidSpawnTimer = 0.0f;
        }
    }

    void SpawnAsteroid()
    {
        // Generate asteroid rotation
        Vector3 asteroidRotation = new Vector3(0.0f, 0.0f, Random.Range(0,360));
        // Spawn Asteroid at random location outside of map
        GameObject generatedAsteroid = Instantiate(GenerateAsteroid(), GenerateAsteroidSpawnPoint(), Quaternion.Euler(asteroidRotation));
        // Generate force
        generatedAsteroid.GetComponent<Particle2D>().AddForceForward(Random.Range(asteroidMinSpeed, asteroidMaxSpeed));
        // Add asteroid to list
        asteroidsList.Add(generatedAsteroid);
    }

    GameObject GenerateAsteroid()
    {
        // Get asteroid prefab
        GameObject generatedAsteroid = asteroidPrefab;
        // Generate asteroid scale
        float asteroidScale = Random.Range(asteroidMinScale, asteroidMaxScale);
        generatedAsteroid.transform.localScale = new Vector3(asteroidScale, asteroidScale, asteroidScale);
        // Set scale
        generatedAsteroid.GetComponent<Particle2D>().radiusOuter = asteroidScale / 2.0f;
        generatedAsteroid.GetComponent<CircleCollisionHull2D>().radius = asteroidScale / 2.0f;
        //Set mass
        generatedAsteroid.GetComponent<Particle2D>().SetMass(asteroidScale);
        // Return generated asteroid
        return generatedAsteroid;
    }

    Vector3 GenerateAsteroidSpawnPoint()
    {
        // Determine edge it will spawn on
        if(Random.Range(0,2) == 0)
        {
            float edge = Random.Range(0,2)*2-1;
            return new Vector3(edge * edgeRight, Random.Range(edgeBot, edgeTop), 0.0f);
        }
        else
        {
            float edge = Random.Range(0,2)*2-1;
            return new Vector3(Random.Range(edgeLeft, edgeRight), edge * edgeTop, 0.0f);
        }
    }

    public void EndGame()
    {
        Debug.Log("GET RECKED");
        gameOverCanvas.enabled = true;
        Destroy(player);
    }
}
