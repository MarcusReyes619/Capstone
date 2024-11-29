using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Transform obj;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacks
    public float timeBetweenAtk;
    public bool isAtk;
    public GameObject attack;
    //States
    public float sightRange, atkRange;
    public bool playerInSightRange, playerInAtkRange;

    bool restirced;
    public bool stun;

    public Animator animtor;

    public Rigidbody rb;
    enum State
    {
        PORTAL,
        CHASE,
        ATTACK,
        HIT,
        STUN,
        DEAD
    }

    //Ragdoll stuff
    private Rigidbody[] _rigdollRigidboodies;

    State currentState;



    //Stats
    public float Hp;


     void Awake()
    {
        player = GameObject.Find("Adam base update").transform;
        agent = GetComponent<NavMeshAgent>();
        _rigdollRigidboodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

   

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
        rb.isKinematic = false;
    }


    // Update is called once per frame
    void Update()
    {
        
        if (Hp > 0)
        {
            //Check for player in sight and in range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAtkRange = Physics.CheckSphere(transform.position, atkRange, whatIsPlayer);

            //Change State
            if (!restirced)
            {
                if (!playerInSightRange && !playerInAtkRange) currentState = State.PORTAL;
                if (playerInSightRange && !playerInAtkRange) currentState = State.CHASE;
                if (playerInSightRange && playerInAtkRange) currentState = State.ATTACK;
            }          
            
            
        }
        else
        {
            currentState = State.DEAD;
        }
        WhatsTheMove();

    }
    #region Ragdoll Stuff
    //Disables the rigdoll 
    void DisableRagdoll()
    {
        foreach(var rig in _rigdollRigidboodies)
        {
            rig.isKinematic = true;
        }
    }
    //enable rigdoll 
    void EnableRagdoll()
    {
        foreach (var rig in _rigdollRigidboodies)
        {
            rig.isKinematic = false;
        }
    }
    #endregion

    //checks the current state and does stuff depending on the state
    void WhatsTheMove()
    {
        //Potral
        switch (currentState)
        {
            //Portal
            case (State.PORTAL):
                if (!walkPointSet)
                {
                    //cal Random point in range
                    float randomZ = Random.Range(-walkPointRange, walkPointRange);
                    float randomX = Random.Range(-walkPointRange, walkPointRange);

                    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

                    //Checks if walk point is on ground
                    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
                }

                if (walkPointSet)
                {
                    agent.SetDestination(walkPoint);
                }            

                //checks if agent reached its destination
                Vector3 distanceToWalkToPoint = transform.position - walkPoint;
                if (distanceToWalkToPoint.magnitude < 1f) walkPointSet = false;
                animtor.SetFloat("Speed", agent.velocity.magnitude);

                break;

            //Chase
            case (State.CHASE):
                agent.SetDestination(player.position);
                break;

            //Atk
            case (State.ATTACK):
                agent.SetDestination(transform.position);
                Vector3 lookHere = new Vector3(player.position.x, 1, player.position.z);
                transform.LookAt(lookHere);
                //Atk code called in the animotr
                if (!isAtk)animtor.SetBool("Atk", true);
                Invoke(nameof(ResetAtk), 1f);      
                break;

            //HIT
            case (State.HIT):
                isAtk = false;
                animtor.SetBool("Hit", restirced);       
                break;
            //STUN
            case (State.STUN):
                restirced = true;

                break;
            //Dead
            case (State.DEAD):
                animtor.enabled = false;
                EnableRagdoll();
               rb.velocity = Vector3.zero;
                rb.isKinematic = false;
                 
          
                Invoke(nameof(Dead), 10f);
                break;
        }
      
   
    }
    #region Attack
    public void Attack()
    {
        isAtk = true;
        restirced = true;
    }


    public void ResetAtk()
    {
        isAtk = false;
        restirced = false;
        animtor.SetBool("Atk", isAtk);
    }
    #endregion

    #region Hit

    //called to reset enemy after getting hit
    public void Recovered()
    {  
        currentState = State.CHASE;
        restirced = false;     
        animtor.SetBool("Hit", restirced);
    }

    //HIT CODE
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Sword>(out Sword enemy))
        {
            if (enemy.isAtk)
            {
               // agent.enabled = false;
                restirced = true;                               
                currentState = State.HIT;    
                Hp -= enemy.dmg;
                Vector3 hitDir = new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z);
                rb.AddForce(hitDir * 5f, ForceMode.Impulse);
             
            }

        }
    }
    #endregion 

    public void Stun()
    {
        stun = true;
        restirced = true;
        animtor.SetBool("Stunned", stun);
        currentState = State.STUN;
    }
    public void StunDone()
    {
        stun = false;
        restirced = false;
        animtor.SetBool("Stunned", stun);
        currentState = State.CHASE;
    }

    void Dead()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
