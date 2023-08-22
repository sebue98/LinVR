using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class checkForObjectHeld : MonoBehaviour
{

    public XRBaseInteractable interactable;

    private bool isHoldingObject = false;

    private void OnGrabbed(XRBaseInteractor interactor)
    {
        isHoldingObject = true;
        Debug.Log("Player grabbed an object.");
    }

    private void OnReleased(XRBaseInteractor interactor)
    {
        isHoldingObject = false;
        Debug.Log("Player released the object.");
    }

    private void Update()
    {
        if (isHoldingObject)
        {
            Debug.Log("Player is holding an object.");
        }
        else
        {
            Debug.Log("Player is not holding an object.");
        }
    }
}
