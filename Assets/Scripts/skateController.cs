using System;
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
    [SerializeField] private BoxCollider noseCollider;
    [SerializeField] private BoxCollider tailCollider;
    [SerializeField] private RailDetector railDetector;

    [Header("Points per trick")]
    [SerializeField] private float ollieMultiplicator;
    [SerializeField] private float kickFlipMultiplicator;
    [SerializeField] private float heelFlipMultiplicator;
    [SerializeField] private float treFlipMultiplicator;
    [SerializeField] private float treHeelFlipMultiplicator;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private float grindDetachDistance = 3f; // tweak to taste
    private bool isGrounded = true;

    private GameObject currentRail;
    private bool isGrinding = false;
    private string gr;
    private Vector3 preGrindPosition;
    private Quaternion preGrindRotation;

    private int grindPoints;




    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        
    }
    void Update()
    {
        if (isGrinding && currentRail != null)
        {
            float distance = Vector3.Distance(transform.position, currentRail.transform.position);
            if (distance > grindDetachDistance)
            {
                EndGrind();
            }
        }
        if (isGrounded)
        {
            if (Input.GetKeyUp(KeyCode.Space))
                Ollie();

            if (Input.GetKeyUp(KeyCode.I))
                KickFlip();

        }


        // Try Nose Slide
        if (Input.GetKeyDown(KeyCode.P) && !isGrounded && !isGrinding)
        {
            NoseSlide();
        }
        // Try Tail Slide
        else if (Input.GetKeyDown(KeyCode.O) && !isGrounded && !isGrinding)
        {
            TailSlide();
        }

        if (isGrinding)
        {
            grindPoints += 10;
        }
        else
        {
            gameManager.AddGrindPointsToScore(grindPoints);
            grindPoints = 0;
        }
        
    }

    private struct TrickRequest
    {
        public string animName;
        public float scoreMultiplier;
    }

    private TrickRequest? queuedTrick = null;

    private void QueueTrick(string animName, float mult)
    {
        queuedTrick = new TrickRequest { animName = animName, scoreMultiplier = mult };
    }

    void FixedUpdate()
    {
        if (queuedTrick.HasValue)
        {
            PerformTrick(queuedTrick.Value);
            queuedTrick = null;
        }

        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * (gravityMultiplier - 1) * rb.mass);
        }
    }

    private bool IsRailOnRight(Transform snapPoint, Collider rail)
    {
        return rail.transform.position.x > 0f; // true = right, false = left
    }

    public void Ollie()
    {
        QueueTrick("Ollie", ollieMultiplicator);
    }

    public void KickFlip()
    {
        QueueTrick("KickFlip", kickFlipMultiplicator);
    }
    public void NoseSlide()
    {
        Collider[] rails = railDetector.DetectRails();
        if (rails.Length > 0)
        {
            bool railOnRight = IsRailOnRight(nosePoint, rails[0]);
            if (railOnRight)
                TryStartGrind(nosePoint, "NoseGrind", rails[0]);
            else
                TryStartGrind(tailPoint, "TailGrind", rails[0]);

        }
    }
    public void TailSlide()
    {
        Collider[] rails = railDetector.DetectRails();
        if (rails.Length > 0)
        {
            bool railOnRight = IsRailOnRight(nosePoint, rails[0]);
            if (railOnRight)
                TryStartGrind(tailPoint, "TailGrind", rails[0]);
            else
                TryStartGrind(nosePoint, "NoseGrind", rails[0]);

        }
    }

    private void PerformTrick(TrickRequest trick)
    {
        if (rb.linearVelocity.y < 0f)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        animator.SetTrigger(trick.animName);

        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel + Vector3.up * jumpForce;

        isGrounded = false;
        gameManager.AddPointsToScore(trick.scoreMultiplier);
    }


    private void TryStartGrind(Transform snapPoint, string animName, Collider rail)
    {
        // Save pre-grind state
        preGrindPosition = transform.position;
        preGrindRotation = transform.rotation;

        gr = animName;
        animator.SetBool(animName, true);

        
        isGrounded = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        currentRail = rail.gameObject;

        StartCoroutine(MoveToRail(snapPoint, rail));
    }

    private IEnumerator MoveToRail(Transform snapPoint, Collider rail)
    {
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;

        // Closest point on rail to the nose/tail
        Vector3 railClosestPoint = rail.ClosestPoint(snapPoint.position);

        // Offset between the root (67) and the snapPoint (Nose/Tail) in world space
        Vector3 snapOffset = transform.position - snapPoint.position;

        // Where the root (67) needs to move so that snapPoint lands exactly on the rail
        Vector3 targetPos = railClosestPoint + snapOffset;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }
        isGrinding = true;
        transform.position = targetPos; // final snap
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
        animator.SetBool(gr, false);
        currentRail = null;

        // Smoothly return to pre-grind position
        StartCoroutine(ReturnFromGrind());
    }

    private IEnumerator ReturnFromGrind()
    {
        float duration = 0.2f; 
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        // Target only X and Z, keep current Y
        Vector3 targetPos = new Vector3(preGrindPosition.x, transform.position.y, preGrindPosition.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Lerp X/Z back, keep Y constant
            float newX = Mathf.Lerp(startPos.x, targetPos.x, t);
            float newZ = Mathf.Lerp(startPos.z, targetPos.z, t);
            transform.position = new Vector3(newX, transform.position.y, newZ);

            // Smoothly return rotation if you want
            transform.rotation = Quaternion.Slerp(startRot, preGrindRotation, t);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = preGrindRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
            isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayers) != 0)
            isGrounded = false;
    }
}
