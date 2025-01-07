using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimtion : MonoBehaviour
{
    [SerializeField] Animator animator;
    AiEnemy enemy;
    [SerializeField] GameObject attack;
    private void Start()
    {
        enemy = GetComponent<AiEnemy>();
    }

    public void Moving(float moveSpeed)
    {
        animator.SetFloat("Speed", moveSpeed);
        
    }
    public void Attack()
    {
        attack.SetActive(true);
        animator.SetBool("Atk", true);
    }

    public void ResetAtackAnmation()
    {
        enemy.ResetAtk();
        attack.SetActive(false);
        animator.SetBool("Atk", false);
    }

    public void Hit()
    {
       
        animator.SetBool("Hit", true);
    }
    public void Recovered()
    {
        enemy.Recovered();
        animator.SetBool("Hit", false);
    }
    public void Stunned()
    {
        animator.SetBool("Stunend", true);

    }
    public void StunnedRecovered()
    {
        animator.SetBool("Stunend", false);
        enemy.StunDone();
    }
    
    public void Dead()
    {
        animator.enabled = false;
    }
    

}
