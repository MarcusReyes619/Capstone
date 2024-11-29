using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimtion : MonoBehaviour
{
    [SerializeField] AiEnemy em;
   
    public void ResetAtackAnmation()
    {
        em.ResetAtk();
    }
    public void Recovered()
    {
        em.Recovered();
    }
    public void StunnedRecovered()
    {
        em.StunDone();
    }
    public void Attack()
    {
        em.Attack();
    }

}
