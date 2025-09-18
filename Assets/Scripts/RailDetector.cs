using UnityEngine;

public class RailDetector : MonoBehaviour
{
    [Header("Rail Detection Settings")]
    [SerializeField] private float detectRadius = 1.5f;               // sphere size
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f); // box size
    [SerializeField] private float detectDistance = 2f;               // legacy downward distance
    [SerializeField] private Vector3 detectOffset = Vector3.zero;     // NEW custom offset
    [SerializeField] private bool useBoxDetection = false;            // toggle between Sphere and Box
    [SerializeField] private LayerMask railLayer;                     // assign "Rail" layer here

    public Collider[] DetectRails()
    {
        Vector3 center = transform.position + detectOffset + Vector3.down * detectDistance;

        if (useBoxDetection)
        {
            return Physics.OverlapBox(
                center,
                boxHalfExtents,
                Quaternion.identity,
                railLayer
            );
        }
        else
        {
            return Physics.OverlapSphere(
                center,
                detectRadius,
                railLayer
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = transform.position + detectOffset + Vector3.down * detectDistance;

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
