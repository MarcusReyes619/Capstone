using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] CanvasGroup ineractableUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ineractableUI.gameObject.SetActive(true);
            LeanTween.cancel(ineractableUI.gameObject);
            LeanTween.alphaCanvas(ineractableUI, 1, 1);
            playerInRange = true;
        }
    }


    private void Update()
    {
        if (playerInRange && Input.GetKeyUp(KeyCode.G))
        {
            Activate();
        }
    }

    public virtual void Activate()
    {
        ineractableUI.gameObject.SetActive(false);

    }

    public virtual void Deactivate()
    {

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            LeanTween.alphaCanvas(ineractableUI, 0, 1).setOnComplete(UiHide);
        }
    }
    void UiHide()
    {

    }
}
