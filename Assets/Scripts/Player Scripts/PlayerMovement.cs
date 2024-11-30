using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Transform cam;

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

   

    Vector3 moveDir;

    public Transform playerObj;

    Rigidbody rb;


    public float HP;


    //RayCast
    public LayerMask knifeLayer;
    RaycastHit knifeFound;
    public GameObject kunai;


    //ragdoll
   // private Rigidbody[] _rigdollRigidboodies;
    public enum StateMove
    {
        FREEZE,
        RUNNING,
        UNLIMITED,
        Hit,
        AIR,
        ATTACK
    }
    public bool freeze;
    public bool unlimited;
    public bool restricted;


    //combat
    bool atkMode = false;
    bool hit;
    public bool isAtk = false;
    int atkAnimation = 0;
    public bool isBlocking;
    
    bool dead = false;

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
            //Jump();
            readyToJump = false;
        
            Invoke(nameof(ResetJump), jumpCoolDown);

        }
        //Attack
        if (Input.GetButtonDown("Fire1"))
        {
            if(atkMode && !isAtk) Attack();          
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (atkMode) KunaiThrow();
        }
        //Block
        if (Input.GetKeyDown(KeyCode.E))
        {
            Block();
        }

        //Attack Mode
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!atkMode)
            {
                atkMode = true;
                animator.SetBool("AttackMode", true);
            }
            else
            {
                animator.SetBool("AttackMode", false);
                atkMode = false;
            }
        }

    }

    #region Movement

    private void MovePlayer()
    {
        //calculate movement Direction
        if (restricted || dead) return;
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

    public void Jump()
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
    #endregion


    /*
     All of combat functions are called inside of the animtion event to 
    the player
     */
    #region Combat

    //Attack
    private void Attack()
    {
        atkAnimation++;
        if (!isAtk)
        {    
            if (atkAnimation > 5) atkAnimation = 1;
            isAtk = true;
            //to stop the basic input controlls till the attack is finished
            restricted = true;


            Vector3 forcedApplyed = playerObj.transform.forward * 5.5f;

            rb.AddForce(forcedApplyed, ForceMode.Impulse); 

            animator.SetInteger("AtkState", atkAnimation);
        }

        Debug.Log(atkAnimation);

    }
    public void ResetAttack()
    {
        isAtk = false;
        restricted = false;
        animator.SetBool("IsAtk", isAtk);
        //sets the animtior back to zero
        animator.SetInteger("AtkState", 0);
    }
    //Block
    void Block()
    {
        isBlocking = true;
        restricted = true;
        animator.SetBool("Blocking", true);
    }

    public void BlockFinished()
    {
        isBlocking = false;
        restricted = false;
        animator.SetBool("Blocking", false);
    }
    void KunaiThrow()
    {
        Instantiate(kunai, orientation.position, cam.rotation);
       
    }
    //Kuni Tellopation
    private void KnifeDection()
    {
        Ray ray = new Ray(cam.transform.position, cam.forward);

        if (Physics.Raycast(ray, out knifeFound, 50f, knifeLayer))
        {
            Debug.Log(knifeFound.collider.gameObject.name + " Hit");
            knifeFound.collider.gameObject.TryGetComponent<Kunai>(out Kunai dectedKunai);
            dectedKunai.light.enabled = true;
            if (Input.GetKeyDown(KeyCode.C))
            {
                transform.position = dectedKunai.transform.position;
                dectedKunai.TelportedTo();
                
            }
        }
        Debug.DrawRay(cam.transform.position, cam.forward, Color.green);

    }
   


    
    #endregion


   

    #region Hit Methods

    void Hit()
    {
        hit = true;
        restricted = true;
        animator.SetBool("Hit", hit);
        
    }
    public void Recovered()
    {
        hit = false;
        restricted = false;
        animator.SetBool("Hit", hit);
       
    }

    void Dead()
    {
        dead = true;
        animator.SetBool("Dead", dead);
    }
    #endregion

    #region unity Functions
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "EnemyAtk")
        {
            other.TryGetComponent<EnemyAttack>(out EnemyAttack emAtk);

            Vector3 hitDir = new Vector3(emAtk.gameObject.transform.position.x, 0, emAtk.gameObject.transform.position.z);
            

            //looks in the dir that was hit
            orientation.LookAt(hitDir);
            playerObj.LookAt(hitDir);
            if (isBlocking)
            {
                emAtk.AttackBlocked();
                Debug.Log("Blocked");
                return;
            }
            if (!hit)
            {                                         
                if (emAtk.em.isAtk)
                {
                    Hit();

                    //resets atk in case player was hit mid atk
                    isAtk = false;

                    //moves playerway from enemy
                    rb.AddForce(hitDir * 5f, ForceMode.Impulse);

                    if (HP >= 0) HP -= 5;
                    else Dead();
                   
                }
            }
        }
    }
    

    private void Awake()
    {
       
     
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
        KnifeDection();
        

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
    #endregion
}
