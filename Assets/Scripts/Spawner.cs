using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> Floor;
    [SerializeField] private List<GameObject> FloorWithObstacles;

    [Header("Spawn Settings")]
    [SerializeField] private float baseTimePerSpawn = 1.5f;
    [SerializeField] private float minTimePerSpawn = 0.5f;
    [SerializeField] private float accelerationRate = 1f; // how fast we ramp up
    [SerializeField] private int MinDistancePerObstacle = 3;
    [SerializeField] private int MaxDistancePerObstacle = 5;

    [Header("Movement")]
    public float Velocity { get; private set; } = 0f; // global velocity floors read
    [SerializeField] private float maxVelocity = 3f;

    private int lastObstacleSpawn = 0;
    private float timePassedSinceLastSpawn = 50;
    private int nextObstacleIn = 1;
    private GameObject lastFloorSpawned;

    private float currentTimePerSpawn;

    private void Awake()
    {
        Transform firstChild = transform.GetChild(0);
        lastFloorSpawned = firstChild.gameObject;
        currentTimePerSpawn = baseTimePerSpawn;
    }

    private void Update()
    {
        // Only run while W or S is held
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            // Accelerate towards max speed
            Velocity = Mathf.MoveTowards(Velocity, maxVelocity, accelerationRate * Time.deltaTime);

            // Update spawn rate proportionally to velocity
            float velocityRatio = Velocity / maxVelocity;
            currentTimePerSpawn = Mathf.Lerp(baseTimePerSpawn, minTimePerSpawn, velocityRatio);

            // Handle spawning
            timePassedSinceLastSpawn += Time.deltaTime;
            if (timePassedSinceLastSpawn > currentTimePerSpawn)
            {
                SpawnFloor();
                timePassedSinceLastSpawn = 0;
            }
        }else
        {
            // Slow down when not pressing W or S
            Velocity = Mathf.MoveTowards(Velocity, 0f, accelerationRate * Time.deltaTime);

            float velocityRatio = Velocity / maxVelocity;
            currentTimePerSpawn = Mathf.Lerp(baseTimePerSpawn, minTimePerSpawn, velocityRatio);
        }
    }

    private void SpawnFloor()
    {
        GameObject nextFloorOrObstacle;
        GameObject spawned;

        if (nextObstacleIn == 0)
        {
            int r = Random.Range(0, FloorWithObstacles.Count);
            nextFloorOrObstacle = FloorWithObstacles[r];
            spawned = Instantiate(nextFloorOrObstacle, this.transform);
            DecideWhenIsNextObstacle();
        }
        else
        {
            int r = Random.Range(0, Floor.Count);
            nextFloorOrObstacle = Floor[r];
            spawned = Instantiate(nextFloorOrObstacle, this.transform);
            nextObstacleIn -= 1;
        }

        Vector3 spawnPosition = lastFloorSpawned.transform.position;
        spawnPosition.z += 0.95f;
        spawned.transform.position = spawnPosition;
        lastFloorSpawned = spawned;
    }

    private void DecideWhenIsNextObstacle()
    {
        nextObstacleIn = Random.Range(MinDistancePerObstacle, MaxDistancePerObstacle);
    }
}
