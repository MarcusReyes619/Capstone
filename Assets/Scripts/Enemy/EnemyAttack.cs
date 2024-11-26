using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] AiEnemy em;
    [SerializeField] BoxCollider collider;
    void Start()
    {
        collider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if enemy is attacking
        if (em.isAtk) collider.isTrigger = true;
        else collider.isTrigger = false;
    }
    
}
