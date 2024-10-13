using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed;

    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultipiler;
    bool readyToJump = true;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    private void MyInput() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            animator.SetBool("Jump", true);
            Invoke(nameof(Jump), 0.25f);
            readyToJump = false;
        
            Invoke(nameof(ResetJump), jumpCoolDown);

        }

    }

    private void MovePlayer()
    {
        //calculate movement Direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        

        if (grounded) rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force); 

        else if(!grounded) rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultipiler, ForceMode.Force);

        //Calculate forward dirction vector
        animator.SetFloat("Speed", rb.velocity.magnitude);

    }

    private void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limits vel if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
    }
    private void ResetJump()
    {
        readyToJump = true;
        animator.SetBool("Jump", false);

    }


    // Start is called before the first frame update
    void Start() 
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update() 
    {
        MyInput();
        SpeedLimit();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        animator.SetBool("OnGround", grounded);
        

        if (grounded) rb.drag = groundDrag; 

        else rb.drag = 0;



    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
}
