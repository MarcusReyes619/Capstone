using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    PlayerMovement pm;
    public bool isClimbing;

    [SerializeField] int rayAmount = 10;

    [SerializeField] float rayLenght = 0.5f;
    [SerializeField] float rayOffset = 0.15f;
    [SerializeField] float rayHeight = 1.7f;

    RaycastHit rayLedgeHit;

    private void Start()
    {
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
        }
    }



}
