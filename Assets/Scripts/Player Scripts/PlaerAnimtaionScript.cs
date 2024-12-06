using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    public void ResetAttack()
    {
        pm.ResetAttack();
    }

    public void Jump()
    {
        pm.Jump();
    }

    public void Recovered()
    {
        pm.Recovered();
    }
    public void BlockFinished()
    {
        pm.BlockFinished();
    }
    public void ThrowKunai()
    {
        pm.KunaiThrow();
    }
    public void ThrowReset()
    {
        pm.ResetThrow();
    }
    public void ThrowHelper()
    {
        pm.animator.SetBool("Throwing", false);
    }
    public void PlayStep()
    {
        pm.PlayStep();
    }
 
}
