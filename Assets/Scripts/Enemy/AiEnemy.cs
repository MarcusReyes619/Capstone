using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform obj;

    [SerializeField] protected LayerMask whatIsGround, whatIsPlayer;

    //Patroling 
    [SerializeField] Vector3 walkPoint;
    protected bool walkPointSet;
    [SerializeField] float walkPointRange;

    //Attacks   
    bool isAtk;
    GameObject attack;
    //States
    [SerializeField] protected float sightRange, atkRange;
     protected bool playerInSightRange, playerInAtkRange;

    bool restirced;
    public bool stun;

    [SerializeField] EnemyAnimtion enemyAnimtion;

    public Rigidbody rb;

    [SerializeField]AudioSource audioSource;
    [SerializeField]AudioClip hitNose;
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


    protected virtual void Awake()
    {
        player = GameObject.Find("Adam base update").transform;
        agent = GetComponent<NavMeshAgent>();
        _rigdollRigidboodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }



    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb.freezeRotation = true;
        rb.isKinematic = false;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        
        if (Hp > 0)
        {
            
            //Check for player in sight and in range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAtkRange = Physics.CheckSphere(transform.position, atkRange, whatIsPlayer);

            //Change State
            if (!restirced)
            {
                if (!playerInSightRange && !playerInAtkRange) Portal();
                if (playerInSightRange && !playerInAtkRange) Chase();
                if (playerInSightRange && playerInAtkRange) Attack();
            }

            //sets the agents movement animation
            enemyAnimtion.Moving(agent.velocity.magnitude);

        }
        else
        {
            currentState = State.DEAD;
        }
       // WhatsTheMove();

    }
    #region Ragdoll Stuff
    //Disables the rigdoll 
    protected virtual void DisableRagdoll()
    {
        foreach(var rig in _rigdollRigidboodies)
        {
            rig.isKinematic = true;
        }
    }
    //enable rigdoll 
    protected virtual void EnableRagdoll()
    {
        foreach (var rig in _rigdollRigidboodies)
        {
            rig.isKinematic = false;
        }
    }
    #endregion

    #region AI States
    protected virtual void Portal()
    {
        currentState = State.PORTAL;
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
            currentState = State.CHASE;
            agent.SetDestination(walkPoint);
   
        }

        //checks if agent reached its destination
        Vector3 distanceToWalkToPoint = transform.position - walkPoint;
        if (distanceToWalkToPoint.magnitude < 1f) walkPointSet = false;
    }

    protected virtual void Chase()
    {
        currentState = State.CHASE;
        agent.SetDestination(player.position);
    }

    protected virtual void Attack()
    {
        currentState = State.ATTACK;

        agent.SetDestination(transform.position);
        Vector3 lookHere = new Vector3(player.position.x, 1, player.position.z);
        transform.LookAt(lookHere);

        isAtk = true;
        restirced = true;
        enemyAnimtion.Attack();

    }
    #endregion

    #region Attack
    //protected virtual void Attack()
    //{
  
       
    //}


    public void ResetAtk()
    {
        isAtk = false;
        restirced = false;
        
    }
    #endregion

    #region Hit

    //called to reset enemy after getting hit
    public void Recovered()
    {  
        currentState = State.CHASE;
        restirced = false;     
      
    }

    //HIT CODE
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
                
                restirced = true;                               
                currentState = State.HIT;    
                
            // Vector3 hitDir = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            //rb.AddForce(hitDir * 2f, ForceMode.Impulse);
            enemyAnimtion.Hit();
            audioSource.PlayOneShot(hitNose);
            

        }
    }
    #endregion 

    public void Stun()
    {
        stun = true;
        restirced = true;
        isAtk = false;
        
        agent.SetDestination(transform.position);
        currentState = State.STUN;
    }
    public void StunDone()
    {
        stun = false;
        restirced = false;
        
        currentState = State.CHASE;
    }

    void Dead()
    {
        Destroy(gameObject);
        EnableRagdoll();
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        Invoke(nameof(Dead), 10f);
    }

    public virtual void ApplyDmg()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
