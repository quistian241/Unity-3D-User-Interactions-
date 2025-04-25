using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteractable : MonoBehaviour, IInteractableRay
{
    public bool isGrabable;

    public string InteractRay() {
        if (isGrabable) {
            return "isGrabable";
        }
        return "isNOTGrabable";
    }
}
