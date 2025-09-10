using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Objects[] objects;
    [SerializeField]
    private float startingTimePerSpawn;
    [SerializeField]
    private float endTimePerSpawn;
    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private float objectTimeAlive;
    [SerializeField]
    private float objectsStartVelocity;
    [SerializeField]
    private float objectsEndVelocity;

    private float totalTimePassed;
    private float timePassedSinceLastSpawn;
    private float timer;
    private float timePerSpawn;

    private void Awake()
    {
        timePerSpawn = startingTimePerSpawn;
    }
    // Update is called once per frame
    void Update()
    {

        totalTimePassed += Time.deltaTime;
        timePassedSinceLastSpawn += Time.deltaTime;

        if (timePassedSinceLastSpawn > timePerSpawn) {
            int randObject = Random.Range(0, (objects.Length));
            int randPosition = Random.Range(0, (spawnPositions.Length));
            GameObject objectSpawned = Instantiate(objects[randObject].Model);
            objectSpawned.transform.position = spawnPositions[randPosition].position;
            timePassedSinceLastSpawn = 0;
        }
    }
}
