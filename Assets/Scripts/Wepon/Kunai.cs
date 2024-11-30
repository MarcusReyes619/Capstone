using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] public Light light;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        light.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
