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

    private bool isGrounded = true;


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

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
        {
            isGrounded = true;
        }
    }
}
