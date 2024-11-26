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
}
