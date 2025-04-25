using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

public class spawnObject : MonoBehaviour
{
    public GameObject xrOrigin;
    public GameObject itemOne;
    public GameObject itemTwo;
    public GameObject rightController;
    public GameObject leftController;
    public InputActionReference triggerActionRefLeft;
    public InputActionReference triggerActionRefRight;
    private Vector3 camPos;
    private Vector3 forward;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Awake()
    {
        SetUpSpawn();
    }

    private void SetUpSpawn() {
        if (triggerActionRefLeft != null && triggerActionRefRight != null)  
        {
            triggerActionRefLeft.action.performed += ctx => UpdateSpawnLeft(ctx.ReadValue<float>());
            triggerActionRefLeft.action.canceled += ctx => UpdateSpawnLeft(0);

            triggerActionRefRight.action.performed += ctx => UpdateSpawnRight(ctx.ReadValue<float>());
            triggerActionRefRight.action.canceled += ctx => UpdateSpawnRight(0);
        }
        else {
            Debug.LogWarning("Input Action References are not set in the Inspector");
        }
    }

    private void UpdateSpawnLeft(float value){
        if (value != 0) {
            // camPos = xrOrigin.transform.position;
            // Instantiate(itemOne, new Vector3(camPos.x, camPos.y, camPos.z), xrOrigin.transform.localRotation);
            Instantiate(itemOne, camPos+3*forward, xrOrigin.transform.localRotation);
        }
    }
    
    private void UpdateSpawnRight(float value){
        if (value != 0) {
            // Vector3 camPos = xrOrigin.transform.position;
            // Instantiate(itemTwo, new Vector3(camPos.x, camPos.y, camPos.z), xrOrigin.transform.localRotation);
            Instantiate(itemTwo, camPos+3*forward, xrOrigin.transform.localRotation);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        camPos = xrOrigin.transform.position;
        forward = xrOrigin.transform.forward;
        // if (Input.GetMouseButtonDown(0)) {
        //     Vector3 camPos = xrOrigin.transform.position;
        //     Instantiate(itemOne, new Vector3(camPos.x, camPos.y, camPos.z+3), xrOrigin.transform.localRotation);
        // }
    }
}
