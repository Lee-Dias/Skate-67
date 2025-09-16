using UnityEngine;

public class RailDetector : MonoBehaviour
{
    [Header("Rail Detection Settings")]
    [SerializeField] private float detectRadius = 1.5f;  // sphere size
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f); // box size
    [SerializeField] private float detectDistance = 2f;  // how far below the skate to check
    [SerializeField] private bool useBoxDetection = false; // toggle between Sphere and Box
    [SerializeField] private LayerMask railLayer;         // assign "Rail" layer here

    public Collider[] DetectRails()
    {
        if (useBoxDetection)
        {
            return Physics.OverlapBox(
                transform.position + Vector3.down * detectDistance,
                boxHalfExtents,
                Quaternion.identity,
                railLayer
            );
        }
        else
        {
            return Physics.OverlapSphere(
                transform.position + Vector3.down * detectDistance,
                detectRadius,
                railLayer
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = transform.position + Vector3.down * detectDistance;

        if (useBoxDetection)
        {
            Gizmos.DrawWireCube(center, boxHalfExtents * 2f);
        }
        else
        {
            Gizmos.DrawWireSphere(center, detectRadius);
        }
    }
}
