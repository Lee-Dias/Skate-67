using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> Floor;
    [SerializeField] private List<GameObject> FloorWithObstacles;

    [Header("Spawn Settings")]
    [SerializeField] private float maxTimePerSpawn = 1.5f; 
    [SerializeField] private float minTimePerSpawn = 0.5f;  
    [SerializeField] private float accelerationRate = 1f;   
    [SerializeField] private float deAccelerationRate = 0.2f;   
    [SerializeField] private int MinDistancePerObstacle = 3;
    [SerializeField] private int MaxDistancePerObstacle = 5;

    [Header("Movement")]
    [SerializeField] private float minVelocity = 0f; 
    [SerializeField] private float maxVelocity = 3f;   
    public float Velocity { get; private set; } = 0f;  

    private int lastObstacleSpawn = 0;
    private float timePassedSinceLastSpawn = 50;
    private int nextObstacleIn = 1;
    private GameObject lastFloorSpawned;

    private float currentTimePerSpawn;
    private bool holding;

    private bool FirstSpawn= true;

    private void Awake()
    {
        currentTimePerSpawn = maxTimePerSpawn;
    }
    public void SetHoldingState(bool hold)
    {
        holding = hold;
    }

    private void Update()
    {
        // only should be active if playing in M&K
        holding = Input.GetKey(KeyCode.W);

        if (holding)
        {

            Velocity = Mathf.MoveTowards(Velocity, maxVelocity, accelerationRate * Time.deltaTime);
        }
        else
        {

            Velocity = Mathf.MoveTowards(Velocity, minVelocity, deAccelerationRate * Time.deltaTime);
        }

        float velocityRatio = Mathf.InverseLerp(minVelocity, maxVelocity, Velocity);
        currentTimePerSpawn = Mathf.Lerp(maxTimePerSpawn, minTimePerSpawn, velocityRatio);

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
        if (FirstSpawn)
        {
            Transform firstChild = transform.GetChild(0);
            lastFloorSpawned = firstChild.gameObject;
            FirstSpawn = false;        
        }

        GameObject nextFloorOrObstacle;
        GameObject spawned;

        if (nextObstacleIn == 0)
        {
            int r = Random.Range(0, FloorWithObstacles.Count);
            nextFloorOrObstacle = FloorWithObstacles[r];
            DecideWhenIsNextObstacle();
        }
        else
        {
            int r = Random.Range(0, Floor.Count);
            nextFloorOrObstacle = Floor[r];
            nextObstacleIn -= 1;
        }


        Vector3 spawnPosition = lastFloorSpawned != null 
            ? lastFloorSpawned.transform.position + new Vector3(0, 0.0001f, 3.5f) 
            : Vector3.zero; 


        spawned = Instantiate(nextFloorOrObstacle, spawnPosition, Quaternion.identity, this.gameObject.transform);

        lastFloorSpawned = spawned;
    }

    private void DecideWhenIsNextObstacle()
    {
        nextObstacleIn = Random.Range(MinDistancePerObstacle, MaxDistancePerObstacle);
    }
    public void levelUp(float multiplier)
    {
        if (minVelocity == 0 && maxVelocity > 1)
        {
            minVelocity = maxVelocity / 4;
        }

        minVelocity *= multiplier;
        maxVelocity *= multiplier;
        maxTimePerSpawn /= multiplier;
        minTimePerSpawn /= multiplier;
    }
}
