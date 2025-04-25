using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleObject : MonoBehaviour, IInteractable
{
    public bool growObject;
    private Transform parent;
    // private BoxCollider parentBox;
    private float scaleFactor = 1.125f;
    public void Interact() {
        Debug.Log(Random.Range(0, 100));
        if (transform.parent != null) {
            if (growObject) {
                parent.localScale *= scaleFactor;
            } else {
                parent.localScale = parent.localScale / scaleFactor;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
    }
}
