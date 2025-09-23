using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class skateController : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;          
    [SerializeField] private float gravityMultiplier = 2f;  
    [Header("References")]
    [SerializeField] private Rigidbody rb;            
    [SerializeField] private Animator animator;           

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers;        
    [SerializeField] private Transform nosePoint;
    [SerializeField] private Transform tailPoint;
    [SerializeField] private BoxCollider noseCollider;
    [SerializeField] private BoxCollider tailCollider;
    [SerializeField] private RailDetector railDetector;

    [Header("Points per trick")]
    [SerializeField] private float ollieMultiplicator;
    [SerializeField] private float bigSpinMultiplicator;
    [SerializeField] private float fsBigSpinMultiplicator;
    [SerializeField] private float kickFlipMultiplicator;
    [SerializeField] private float heelFlipMultiplicator;
    [SerializeField] private float treFlipMultiplicator;
    [SerializeField] private float treHeelFlipMultiplicator;
    [SerializeField] private float fsTreFlipMultiplicator;
    [SerializeField] private float fsTreHeelFlipMultiplicator;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private float grindDetachDistance = 3f; 
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
        if (isGrounded && !isGrinding)
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
                Ollie();
            if (Input.GetKeyUp(KeyCode.Alpha2))
                BigSpin();
            if (Input.GetKeyUp(KeyCode.Alpha3))
                FsBigSpin();
            if (Input.GetKeyUp(KeyCode.Alpha4))
                KickFlip();
            if (Input.GetKeyUp(KeyCode.Alpha5))
                HeelFlip();
            if (Input.GetKeyUp(KeyCode.Alpha6))
                TreFlip();
            if (Input.GetKeyUp(KeyCode.Alpha7))
                TreHeelFlip();
            if (Input.GetKeyUp(KeyCode.Alpha8))
                FsTreFlip();
            if (Input.GetKeyUp(KeyCode.Alpha9))
                FsTreHeelFlip();

            

        }



        if (Input.GetKeyDown(KeyCode.P) && !isGrounded && !isGrinding)
        {
            NoseSlide();
        }
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
        return rail.transform.position.x > 0f; 
    }

    public void Ollie()
    {
        QueueTrick("Ollie", ollieMultiplicator);
    }
    public void BigSpin()
    {
        QueueTrick("BigSpin", bigSpinMultiplicator);
    }
    public void FsBigSpin()
    {
        QueueTrick("FsBigSpin", fsBigSpinMultiplicator);
    }

    public void KickFlip()
    {
        QueueTrick("KickFlip", kickFlipMultiplicator);
    }
    public void HeelFlip()
    {
        QueueTrick("HeelFlip", heelFlipMultiplicator);
    }
    public void TreFlip()
    {
        QueueTrick("TreFlip", treFlipMultiplicator);
    }
    public void TreHeelFlip()
    {
        QueueTrick("TreHeelFlip", treHeelFlipMultiplicator);
    }
    public void FsTreFlip()
    {
        QueueTrick("FsTreFlip", fsTreFlipMultiplicator);
    }
    public void FsTreHeelFlip()
    {
        QueueTrick("FsTreHeelFlip", fsTreHeelFlipMultiplicator);
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
        if (!isGrounded || isGrinding){return;}
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


        Vector3 railClosestPoint = rail.ClosestPoint(snapPoint.position);


        Vector3 snapOffset = transform.position - snapPoint.position;

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

        StartCoroutine(ReturnFromGrind());
    }

    private IEnumerator ReturnFromGrind()
    {
        float duration = 0.2f; 
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;


        Vector3 targetPos = new Vector3(preGrindPosition.x, transform.position.y, preGrindPosition.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);


            float newX = Mathf.Lerp(startPos.x, targetPos.x, t);
            float newZ = Mathf.Lerp(startPos.z, targetPos.z, t);
            transform.position = new Vector3(newX, transform.position.y, newZ);

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
