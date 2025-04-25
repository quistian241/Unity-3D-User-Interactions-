using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Rendering;

interface IInteractableRay {
    public string InteractRay();
}
public class DetectInteractable : MonoBehaviour
{
    private Ray rayRight;
    public LineRenderer indicator;
    public GameObject rightController;
    // white color
    private Color grabObj = Color.white; 
    // purple for size change
    private Color sizeChangeObj = Color.magenta;
    // red for nothing to interact with 
    private Color noObjDetected = Color.red;
    // Start is called before the first frame update
    private GameObject highlighted = null;
    private Color[] originalColors;
    public GameObject plane;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rayRight = new Ray(rightController.transform.position, rightController.transform.forward);
        indicator.positionCount = 2;
        indicator.SetPosition(0, rightController.transform.position);
        indicator.SetPosition(1, rightController.transform.position+rightController.transform.forward*10f);
        
        if (Physics.Raycast(rayRight, out RaycastHit hitinfoRight)) {
            if (hitinfoRight.transform.gameObject == plane) {
                if (highlighted != null) {
                    removeHighlight(highlighted);
                    highlighted = null;
                }
            }
            else if (highlighted == null) {
                highlighted = hitinfoRight.transform.gameObject;
                highlight(highlighted);
            }
            else if (highlighted != hitinfoRight.transform.gameObject) {
                removeHighlight(highlighted);
                highlighted = hitinfoRight.transform.gameObject;
                highlight(highlighted);
            }
            
            if (hitinfoRight.collider.gameObject.TryGetComponent(out IInteractableRay interactObj)) {
                string objType = interactObj.InteractRay();
                if (objType == "isGrabable") {
                    indicator.startColor = grabObj;
                    indicator.endColor = grabObj;
                } else {
                    indicator.startColor = sizeChangeObj;
                    indicator.endColor = sizeChangeObj;
                }
                // indicator
            }
        } else {
            if (highlighted != null) {
                removeHighlight(highlighted);
                highlighted = null;
            }
            indicator.startColor = noObjDetected;
            indicator.endColor = noObjDetected;
        }
    }

    private void highlight(GameObject highlightable) {
        Material[] materials = highlightable.GetComponent<Renderer>().materials;
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++) {
            originalColors[i] = materials[i].color;
            materials[i].color = Color.red;
        }
    }

    private void removeHighlight(GameObject highlighted) {
        Material[] materials = highlighted.GetComponent<Renderer>().materials;
        for (int i = 0; i < materials.Length; i++) {
            materials[i].color = originalColors[i];
        }
    }
}
