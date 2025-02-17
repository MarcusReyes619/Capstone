using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public enum PlayerState {NORMAL, CLIMBING }

public class PlayerClimb : MonoBehaviour
{
    PlayerMovement pm;

    public PlayerState playerState;
    public bool isClimbing;

    [SerializeField] int rayAmount = 10;

    [SerializeField] float rayLenght = 0.5f;
    [SerializeField] float rayOffset = 0.15f;
    [SerializeField] float rayHeight = 1.7f;

    RaycastHit rayLedgeHit;
    [SerializeField] LayerMask ledgeLayer;

    [Space(5)]
    public GameObject climbObj;

    private void Start()
    {
        playerState = PlayerState.NORMAL;
        pm = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if(isClimbing && pm.grounded)
        {
            CheckOnGroundRay();
            Debug.Log("ture");
        }
    }

    private void CheckOnGroundRay()
    {
        for(int i = 0; i < rayAmount; i++)
        {
            Vector3 rayPos = transform.position + Vector3.up * rayHeight + Vector3.up * rayOffset * i;

            Debug.DrawRay(rayPos, transform.forward, Color.cyan);

            if(Physics.Raycast(rayPos, transform.forward, out rayLedgeHit, rayLenght , ledgeLayer, QueryTriggerInteraction.Ignore))  
            {
                climbObj = rayLedgeHit.transform.gameObject;
                if (Input.GetKeyDown(KeyCode.C))
                {
                    isClimbing = true;
                  //  StartCoroutine(GrabLedge());
                }
                break;
            }
        }
    }

    IEnumerable GrabLedge()
    {
        pm.animator.CrossFade("Brace", 0.2f);
        yield return null;
    }



}
