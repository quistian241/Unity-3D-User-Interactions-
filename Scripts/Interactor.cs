using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

interface IInteractable_0 {
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange; 
    public GameObject leftController;
    public GameObject rightController;
    public InputActionReference triggerActionRefLeft;
    public InputActionReference triggerActionRefRight;

    private Ray rayLeft;
    private Ray rayRight;

    // public void Awake()
    // {
    //     SetUpInteract();
    // }
    void Update() {
        SetUpInteract();
    }

    private void SetUpInteract() {
        if (triggerActionRefLeft != null && triggerActionRefRight != null)  
        {
            triggerActionRefLeft.action.performed += ctx => ClickInteractorLeft(ctx.ReadValue<float>());
            triggerActionRefLeft.action.canceled += ctx => ClickInteractorLeft(0);

            triggerActionRefRight.action.performed += ctx => ClickInteractorRight(ctx.ReadValue<float>());
            triggerActionRefRight.action.canceled += ctx => ClickInteractorRight(0);
        }
        else {
            Debug.LogWarning("Input Action References are not set in the Inspector");
        }
    }

    private void ClickInteractorLeft(float value){
        if (value != 0) {
            rayLeft = new Ray(leftController.transform.position, leftController.transform.forward);
            if (Physics.Raycast(rayLeft, out RaycastHit hitinfoLeft, InteractRange)) {
                if (hitinfoLeft.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                    interactObj.Interact();
                }
            }
        }
    }
    
    private void ClickInteractorRight(float value){
        if (value != 0) {
            rayRight = new Ray(rightController.transform.position, rightController.transform.forward);
            if (Physics.Raycast(rayRight, out RaycastHit hitinfoRight, InteractRange)) {
                if (hitinfoRight.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                    interactObj.Interact();
                }
            }
        }
    }
}
