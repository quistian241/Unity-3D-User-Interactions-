using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class teleportation : MonoBehaviour
{
    public GameObject leftController;
    public GameObject origin;
    public InputActionReference teleportActionRef;
    public InputActionReference moveActionRef;
    public LineRenderer indicator;
    // Start is called before the first frame update
    void Start()
    {
        indicator = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        indicator.positionCount = 2;
        indicator.SetPosition(0, leftController.transform.position);
        indicator.SetPosition(1, leftController.transform.position+leftController.transform.forward*10f);
    }
    public void Awake()
    {
        SetUpTeleport();
    }

    private void SetUpTeleport() {
        if (teleportActionRef != null && moveActionRef != null)  
        {
            // teleportActionRef.action.performed += ctx => UpdateHandAnimation("Trigger", ctx.ReadValue<float>());
            // teleportActionRef.action.canceled += ctx => UpdateHandAnimation("Trigger", 0);

            moveActionRef.action.performed += ctx => UpdateTeleport(ctx.ReadValue<float>());
            moveActionRef.action.canceled += ctx => UpdateTeleport(0);
        }
        else {
            Debug.LogWarning("Input Action References are not set in the Inspector");
        }
    }

    private void UpdateTeleport(float value){
        RaycastHit hit;
        if (value!= 0 && Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit)) {
            origin.transform.position = new Vector3(hit.point[0], origin.transform.position.y, hit.point[2]);
        }
    }

    private void OnEnable()
    {
        teleportActionRef?.action.Enable();
        moveActionRef?.action.Enable();
    }

    private void OnDisable()
    {
        teleportActionRef?.action.Disable();
        moveActionRef?.action.Disable();
    }
}
