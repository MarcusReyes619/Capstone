using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultipiler;
    bool readyToJump;
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

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

        }

    }

    private void MovePlayer()
    {
        //calculate movement Direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!grounded) rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultipiler, ForceMode.Force);

        else
        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        
        
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
    }


    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update() 
    {
        MyInput();
        SpeedLimit();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (!grounded)rb.drag = 0;

        else rb.drag = groundDrag;
         
        

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
}
