using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float dmg;
    public PlayerMovement pm;

    //check if attacking
    public bool isAtk;

    void Start()
    {
        
    }

    
    void Update()
    {
        isAtk = pm.isAtk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(pm.isAtk == true && other.gameObject.TryGetComponent<TestEnemy>(out TestEnemy enemy))
        {
            enemy.Test();
        }
    }
}
