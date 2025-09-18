using UnityEngine;

public class FloorController : MonoBehaviour
{
    private Spawner spawner;

    private void Start()
    {
        spawner = FindFirstObjectByType<Spawner>();
    }

    void Update()
    {
        if (spawner == null) return;

        Vector3 pos = transform.position;
        pos.z -= spawner.Velocity * Time.deltaTime;
        transform.position = pos;

        if (transform.position.z < -25)
        {
            Destroy(gameObject);
        }
    }
}
