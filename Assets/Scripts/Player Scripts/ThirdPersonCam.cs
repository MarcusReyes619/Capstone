using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [SerializeField]public Transform orientation;
    [SerializeField] public Transform player;
    [SerializeField] public Transform playerObj;
    [SerializeField] public Rigidbody rb;

    public float rotationSpeed;

    //float horizontalInput;
    //float verticalInput;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //void OnCamMove(InputValue value)
    //{
    //    horizontalInput = value.Get<Vector2>().x;
    //    verticalInput = value.Get<Vector2>().y;
    //    Debug.Log(horizontalInput);
    //    Debug.Log(verticalInput);
    //}
    // Update is called once per frame
    void Update()
    {
        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //roate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

       

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

        
    }
}
