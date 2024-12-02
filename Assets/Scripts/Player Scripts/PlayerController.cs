using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] PlayerMovement pm;
    [SerializeField] ThirdPersonCam thridPcam;

    public GameObject thirdPersonCam;
    public GameObject CutSceneCam;
    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        pm.enabled = false;
        thridPcam.enabled = false;
    }

    public void Dectivate()
    {
        pm.enabled = true;
        thridPcam.enabled = true;
    }


}
