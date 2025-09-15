using UnityEngine;

public class FloorController : MonoBehaviour
{
    private float velocity = 1.5f;

    public void ChangVelocity(float level)
    {
        velocity += level;   
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z -= velocity * Time.deltaTime;
        transform.position = pos;

        if (this.transform.position.z < -22)
        {
            Destroy(gameObject);
        }
    }
}
