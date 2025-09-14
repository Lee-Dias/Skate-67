using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Floor;
    [SerializeField]
    private List<GameObject> FloorWithObstacles;
    [SerializeField]
    private float timePerSpawn = 1.5f;
    [SerializeField]
    private int MinDistancePerObstacle = 3;
    [SerializeField]
    private int MaxDistancePerObstacle = 5;

    private int lastObstacleSpawn = 0;
    private float timePassedSinceLastSpawn = 50;
    private int nextObstacleIn = 1;
    private GameObject lastFloorSpawned;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Transform firstChild = transform.GetChild(0);
        lastFloorSpawned = firstChild.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        timePassedSinceLastSpawn += Time.deltaTime;
        if (timePassedSinceLastSpawn > timePerSpawn)
        {
            GameObject nextFloorOrObstacle;
            GameObject spawned;
            if (nextObstacleIn == 0)
            {
                int r = Random.Range(0, FloorWithObstacles.Count());
                nextFloorOrObstacle = FloorWithObstacles[r];
                spawned = Instantiate(nextFloorOrObstacle, this.transform);
                DecideWhenIsNextObstacle();
            }
            else
            {
                int r = Random.Range(0, Floor.Count());
                nextFloorOrObstacle = Floor[r];
                spawned = Instantiate(nextFloorOrObstacle, this.transform);
                nextObstacleIn -= 1;
            }

            Vector3 SpawnPositon = lastFloorSpawned.transform.position;
            SpawnPositon.z += 0.95f;
            spawned.transform.position = SpawnPositon;
            lastFloorSpawned = spawned;
            timePassedSinceLastSpawn = 0;   
        }
    }

    private void DecideWhenIsNextObstacle() {
        nextObstacleIn = Random.Range(MinDistancePerObstacle, MaxDistancePerObstacle);
    }

    public void LevelChanged() {
        
    }
}
