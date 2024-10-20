using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{

    [Header("References")]
    public PlayerMovement pm;
    public Transform orientation;
    public Transform cam;
    public Rigidbody rb;

    [Header("Ledge Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabRange;

    public bool holding;

    [Header("Ledge Jumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public float ledgeJumpForwardForce;
    public float ledgeJumpUpwardForce;


    [Header("Ledge Detection")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private Transform lastLedge;
    private Transform currentLedge;

    private RaycastHit ledgeHit;

    [Header("Exit Ledge")]
    public bool exitingLedge;
    public float exitLedgeTime;
    private float exitLedgeTimer;


    

    private void LedgeDetection()
    {
         Vector3 SphareCast = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
         bool ledgeDetected = Physics.SphereCast(SphareCast, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength,
            whatIsLedge);

        if (!ledgeDetected) return;

        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);
       

        if (ledgeHit.transform == lastLedge) return;

        if (distanceToLedge < maxLedgeGrabRange && !holding)  EnterLedgeHold();  
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 SphareCast = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Gizmos.DrawWireSphere(SphareCast, ledgeSphereCastRadius);
    }

    private void SubStateMachine()
    {
        float horInput = Input.GetAxisRaw("Horizontal");
        float vertInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horInput != 0 || vertInput != 0;


        //Substate 1 -Holding onto ledge
        if (holding)
        {
            FreezeRigidbodyOnLedge();

            if (anyInputKeyPressed) ExitLedgeHold();

            if (Input.GetKeyDown(jumpKey)) LedgeJump();
        }
        else if (exitingLedge)
        {
            if (exitLedgeTime > 0) exitLedgeTime -= Time.deltaTime;
            else exitingLedge = false;
        }

    }
    private void EnterLedgeHold()
    {
        holding = true;

        pm.unlimited = true;
        pm.restricted = true;


        currentLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        
    }

    private void FreezeRigidbodyOnLedge()
    {
        
        rb.useGravity = false;

        Vector3 directionToLedge = currentLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currentLedge.position);

        //move player to ledge
        if(distanceToLedge > 1f)
        {
            if (rb.velocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }
        //Hold onto ledge
        else
        {
            if (!pm.freeze) pm.freeze = true;
            if(pm.unlimited) pm.unlimited = false;
                     
        }
        if (distanceToLedge > maxLedgeGrabRange) ExitLedgeHold();
    }

    private void ExitLedgeHold()
    {
        exitingLedge = true;
        exitLedgeTimer = exitLedgeTimer;

        holding = false;

        pm.restricted = false;
        pm.freeze = false;
        pm.unlimited = false;

        rb.useGravity = true;

        StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);
    }

    private void ResetLastLedge()
    {
        lastLedge = null;
    }

    private void LedgeJump()
    {
        ExitLedgeHold();
       // Invoke(nameof(DelayedJumpForce), 0.05f);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 forceToAdd = moveDir * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;
        rb.velocity = Vector3.zero;
        rb.AddForce(forceToAdd, ForceMode.Impulse);

        //rb.velocity = rb.velocity + forceToAdd;
    }

    private void DelayedJumpForce()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 forceToAdd = moveDir * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;
        rb.velocity = Vector3.zero;
        // rb.AddForce(forceToAdd, ForceMode.Impulse);

        rb.velocity = rb.velocity + forceToAdd;
        
        Debug.Log(rb.velocity);
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LedgeDetection();
        SubStateMachine();
        
    }
}
