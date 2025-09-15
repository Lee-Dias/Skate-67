using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class skateController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;          // Upward pop
    [SerializeField] private float gravityMultiplier = 2f;  // Makes jump fall faster (snappier)

    [Header("References")]
    [SerializeField] private Rigidbody rb;                  // Rigidbody on PlayerRoot
    [SerializeField] private Animator animator;             // Animator for visual mesh

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers;        // Floor + obstacles layers
    [SerializeField] private Transform nosePoint;
    [SerializeField] private Transform tailPoint;
    [SerializeField] private RailDetector railDetector;

    private bool isGrounded = true;
    private bool canGrind = false;
    private GameObject currentRail;
    private bool isGrinding = false;
    private string gr;


    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Jump input (only if grounded)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PerformOllie();
        }
        // Try grind
        if (Input.GetKeyDown(KeyCode.P) && !isGrounded && !isGrinding)
        {
            Collider[] rails = railDetector.DetectRails();  
            if (rails.Length > 0)
            {
                TryStartGrind(nosePoint, "NoseGrind", rails[0]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.O) && !isGrounded && !isGrinding)
        {
            Collider[] rails = railDetector.DetectRails();  
            if (rails.Length > 0)
            {
                TryStartGrind(tailPoint, "TailGrind", rails[0]);
            }
        }
    }

    void FixedUpdate()
    {
        // Apply stronger gravity for snappier fall
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1) * rb.mass);
        }
    }

    private void PerformOllie()
    {
        // Physics-based jump
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        animator.SetTrigger("Ollie");
        isGrounded = false;
    }
    private void TryStartGrind(Transform snapPoint, string animName, Collider rail)
    {
        gr = animName;
        animator.SetBool(animName, true);

        isGrinding = true;
        isGrounded = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        currentRail = rail.gameObject;

        // Start a coroutine to move toward the rail
        StartCoroutine(MoveToRail(snapPoint, rail));
    }

    private IEnumerator MoveToRail(Transform snapPoint, Collider rail)
    {
        float duration = 0.2f; // time to reach the rail
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Bounds railBounds = rail.bounds;

        // Target position so snapPoint aligns with rail top and horizontally centered
        Vector3 targetPos = startPos;
        targetPos.y = railBounds.max.y + (transform.position.y - snapPoint.position.y);
        targetPos.x = railBounds.center.x - (snapPoint.position.x - transform.position.x);
        targetPos.z = railBounds.center.z - (snapPoint.position.z - transform.position.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos; // ensure exact alignment
    }
    private void OnTriggerExit(Collider other)
    {
        if (isGrinding && other.gameObject == currentRail)
        {
            EndGrind();
        }
    }

    private void EndGrind()
    {
        isGrinding = false;
        rb.useGravity = true;
        animator.SetBool(gr, false); // or any normal skating anim
        currentRail = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
        {
            isGrounded = true;
        }
    }
}
