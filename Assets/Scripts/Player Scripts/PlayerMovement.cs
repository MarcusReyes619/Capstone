using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    [Header("Movment")]
    public float moveSpeed;
    public float groundDrag;

    private float currentSpeed;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultipiler;
    public bool readyToJump = true;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    //Attack
    public bool isAtk = false;
    int atkAnimation = 0;

    Vector3 moveDir;

    Rigidbody rb;

    public enum StateMove
    {
        FREEZE,
        RUNNING,
        UNLIMITED,
        AIR,
        ATTACK
    }


    public bool freeze;
    public bool unlimited;

    public bool restricted;

    StateMove currentState;

    private void StateHandler()
    {

        if (freeze)
        {
            currentState = StateMove.FREEZE;
            rb.velocity = Vector3.zero;

        }
        else if (unlimited)
        {
            currentState = StateMove.UNLIMITED;
            currentSpeed = 999f;
            return;
        }
        else    
            currentState = StateMove.RUNNING;
            currentSpeed = moveSpeed;      
    }

    private void MyInput() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded && currentState != StateMove.AIR)
        {
            animator.SetBool("Jump", true);
            //Invoke(nameof(Jump), 0.25f);
            Jump();
            readyToJump = false;
        
            Invoke(nameof(ResetJump), jumpCoolDown);

        }
        //Attack
        if (Input.GetButtonDown("Fire1"))
        {
           Attack();
           Invoke(nameof(RestAttack), 1f);

        }

    }

    private void MovePlayer()
    {
        //calculate movement Direction
        if (restricted) return;
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if (grounded) rb.AddForce(moveDir.normalized * currentSpeed * 10f, ForceMode.Force); 

        //in air
        else if(!grounded) rb.AddForce(moveDir.normalized * currentSpeed * 10f * airMultipiler, ForceMode.Force);

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
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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

    private void Attack()
    {
        isAtk = true;
        //to stop the basic input controlls till the attack is finished
        restricted = true;

        // rb.AddForce(, ForceMode.Impulse);
        
        atkAnimation++;

        if(atkAnimation >= 5)
        {
            //rest animations
            atkAnimation = 0;
            //attack animtion

        }

       

        animator.SetBool("IsAtk", isAtk);   

    }

    private void RestAttack()
    {
        isAtk = false;

        restricted = false;
        animator.SetBool("IsAtk", isAtk);
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
        StateHandler();



        //ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        animator.SetBool("OnGround", grounded);


        if (grounded) 
        { 
            rb.drag = groundDrag; 
        }

        else rb.drag = 0;

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
}
