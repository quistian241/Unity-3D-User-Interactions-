using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

interface IInteractable {
    public void Interact();
}
public class playerStateChange : MonoBehaviour
{
    public enum playerStatus{
        teleportSize = 0,
        spawnObj = 1
    }; 
    public playerStatus currentState = playerStatus.teleportSize;
    public Transform InteractorSource;
    public float InteractRange; 
    public GameObject leftController;
    public GameObject rightController;
    public GameObject origin;

    // trigger = top buttons
    public InputActionReference triggerActionRefLeft;
    public InputActionReference triggerActionRefRight;
    
    // the grip on Left con
    public InputActionReference gripActionRefLeft;
    public InputAction gripActionLeft;

    private Ray rayLeft;
    private Ray rayRight;
    public GameObject spawnOrigin;
    public GameObject itemOne;
    public GameObject itemTwo;
    private Vector3 camPos;
    private Vector3 forward;
    public LineRenderer indicator;
    public Text stateText;

    void Start()
    {
        indicator = GetComponent<LineRenderer>();
    }
    
    private void controlPlayerActions() {
        if (triggerActionRefLeft != null && triggerActionRefRight != null)  
        {
            triggerActionRefLeft.action.performed += ctx => leftTriggerPressed(ctx.ReadValue<float>());
            triggerActionRefLeft.action.canceled += ctx => leftTriggerPressed(0);

            triggerActionRefRight.action.performed += ctx => rightTriggerPressed(ctx.ReadValue<float>());
            triggerActionRefRight.action.canceled += ctx => rightTriggerPressed(0);
        }
        else {
            Debug.LogWarning("Input Action References are not set in the Inspector");
        }
    }

    int strTimerDuration = 180;
    private void strTime(){
        if (strTimerDuration > 0) {
            strTimerDuration--;
        }
        else {
            stateText.text = "";
        }
    }

    private void updatePlayerState(){
        if (currentState == playerStatus.teleportSize) {
            currentState = playerStatus.spawnObj;
            stateText.text = "Spawn Object State";
            strTimerDuration = 20;
        }

        else if (currentState == playerStatus.spawnObj){ 
            currentState = playerStatus.teleportSize;
            stateText.text = "Teleport & Scale State";
            strTimerDuration = 20;
        }
        else {
            // stateText.text = Timer(fixedDuration);
        }
    }

    
    private void leftTriggerPressed(float value) {
        if (value != 0) {
            RaycastHit hit;
            if (currentState == playerStatus.teleportSize && Physics.Raycast(rayLeft, out hit)){
                origin.transform.position = new Vector3(hit.point[0], origin.transform.position.y, hit.point[2]);
            }
            else if (currentState == playerStatus.spawnObj){ 
                Instantiate(itemOne, camPos+3*forward, spawnOrigin.transform.localRotation);
            }
            
        }
    }

    private void rightTriggerPressed(float value) {
        if (value != 0) {
            if (currentState == playerStatus.teleportSize && Physics.Raycast(rayRight, out RaycastHit hitinfoRight, InteractRange)){
                if (hitinfoRight.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                    interactObj.Interact();
                }
            } 
            else if (currentState == playerStatus.spawnObj){ 
                Instantiate(itemTwo, camPos+3*forward, spawnOrigin.transform.localRotation);
            }
            
        }
    }

    private bool wasPressedLastFrame = false;
    private float pressThreshold = 0.5f;
    // Update is called once per frame
    void Update()
    {
        // rays for all sections to use 
        rayLeft = new Ray(leftController.transform.position, leftController.transform.forward);
        rayRight = new Ray(rightController.transform.position, rightController.transform.forward);

        // camera for spawning ojects 
        camPos = spawnOrigin.transform.position;
        forward = spawnOrigin.transform.forward;

        // indicator for teleporting 
        indicator.positionCount = 2;
        indicator.SetPosition(0, leftController.transform.position);
        indicator.SetPosition(1, leftController.transform.position+leftController.transform.forward*10f);

        float gripvalue = gripActionLeft.ReadValue<float>();
        bool isPressedNow = gripvalue > pressThreshold;
        if (isPressedNow && !wasPressedLastFrame) {
            updatePlayerState();
        }
        wasPressedLastFrame = isPressedNow;

        controlPlayerActions();

        strTime();
    }

    private void OnEnable()
    {
        gripActionLeft.Enable();
        gripActionRefLeft?.action.Enable();
        triggerActionRefLeft?.action.Enable();
        triggerActionRefRight?.action.Enable();
    }

    private void OnDisable()
    {
        gripActionLeft.Disable();
        gripActionRefLeft?.action.Disable();
        triggerActionRefLeft?.action.Disable();
        triggerActionRefRight?.action.Disable();
    }
}
