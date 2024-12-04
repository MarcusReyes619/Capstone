using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SignalReceiver))]
public class CutSceneStart : Interactable
{
    [SerializeField] GameObject cutSceneStart;
    public override void Activate()
    {
        base.Activate();
        cutSceneStart.SetActive(true);
        PlayerController.instance.CutSceneCam.SetActive(true);
        PlayerController.instance.thirdPersonCam.SetActive(false);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        cutSceneStart.SetActive(false);
        PlayerController.instance.CutSceneCam.SetActive(false);
        PlayerController.instance.thirdPersonCam.SetActive(true);
        Debug.Log("cutover");
    }
}
