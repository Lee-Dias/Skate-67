using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> Floor;
    [SerializeField] private List<GameObject> FloorWithObstacles;

    [Header("Spawn Settings")]
    [SerializeField] private float maxTimePerSpawn = 1.5f; // slowest spawn interval
    [SerializeField] private float minTimePerSpawn = 0.5f;  // fastest spawn interval
    [SerializeField] private float accelerationRate = 1f;   // how fast we ramp up
    [SerializeField] private int MinDistancePerObstacle = 3;
    [SerializeField] private int MaxDistancePerObstacle = 5;

    [Header("Movement")]
    [SerializeField] private float minVelocity = 0f; // minimum movement speed
    [SerializeField] private float maxVelocity = 3f;   // maximum movement speed
    public float Velocity { get; private set; } = 0f;  // current global velocity

    private int lastObstacleSpawn = 0;
    private float timePassedSinceLastSpawn = 50;
    private int nextObstacleIn = 1;
    private GameObject lastFloorSpawned;

    private float currentTimePerSpawn;

    private void Awake()
    {
        Transform firstChild = transform.GetChild(0);
        lastFloorSpawned = firstChild.gameObject;
        currentTimePerSpawn = maxTimePerSpawn;
    }

    private void Update()
    {
        bool holding = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);

        if (holding)
        {
            // Accelerate towards max speed
            Velocity = Mathf.MoveTowards(Velocity, maxVelocity, accelerationRate * Time.deltaTime);
        }
        else
        {
            // Decelerate towards min velocity (not all the way to zero)
            Velocity = Mathf.MoveTowards(Velocity, minVelocity, accelerationRate * Time.deltaTime);
        }

        // Map velocity → spawn interval
        float velocityRatio = Mathf.InverseLerp(minVelocity, maxVelocity, Velocity);
        currentTimePerSpawn = Mathf.Lerp(maxTimePerSpawn, minTimePerSpawn, velocityRatio);

        // Spawn floors whenever we’re moving (Velocity > 0), not just when holding
        if (Velocity > 0f)
        {
            timePassedSinceLastSpawn += Time.deltaTime;
            if (timePassedSinceLastSpawn > currentTimePerSpawn)
            {
                SpawnFloor();
                timePassedSinceLastSpawn = 0;
            }
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
    public void levelUp(float multiplier)
    {
        if (minVelocity == 0)
        {
            minVelocity = maxVelocity / 4;
        }

        minVelocity *= multiplier;
        maxVelocity *= multiplier;
        maxTimePerSpawn /= multiplier;
        minTimePerSpawn /= multiplier;
    }
}
