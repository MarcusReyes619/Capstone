using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] public Light light;
    private bool targetHit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit) return;
        else targetHit = true;

        //makes sure projectile sticks to surface
        rb.isKinematic = true;

        //make sire projectile moves with traget
        transform.SetParent(collision.transform);
    }

    public void Spotted() 
    {
        light.enabled = true;
    }
    //called when player TPs to the kunai
    public void TelportedTo()
    {
        Destroy(this.gameObject);
    }
}
