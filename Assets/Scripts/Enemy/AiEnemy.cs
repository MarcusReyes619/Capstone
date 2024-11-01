using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacks
    public float timeBetweenAtk;
    bool isAtk;

    //States
    public float sightRange, atkRange;
    public bool playerInSightRange, playerInAtkRange;
    enum State
    {
        PORTAL,
        CHASE,
        ATTACK,
        DEAD
    }

    State currentState;

    Animator animator;

    public float Hp;


     void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check for player in sight and in range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAtkRange = Physics.CheckSphere(transform.position, atkRange, whatIsPlayer);

        //Change State
        if (!playerInSightRange && !playerInAtkRange) currentState = State.PORTAL;
        if (!playerInSightRange && playerInAtkRange) currentState = State.CHASE;
        if (playerInSightRange && playerInAtkRange) currentState = State.ATTACK;



    }

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
                

                break;

            //Chase
            case (State.CHASE):
                agent.SetDestination(player.position);
                break;

            //Atk
            case (State.ATTACK):
                //Atk code here

                agent.SetDestination(transform.position);
                transform.LookAt(player);
                break;
            
            //Dead
            case (State.DEAD):
                break;


        }
    }

   
}
