using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlight : MonoBehaviour
{
    public GameObject item;
    private Color[] originalColors;
    private Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        materials = item.GetComponent<Renderer>().materials;
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++) {
            originalColors[i] = materials[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            materials = item.GetComponent<Renderer>().materials;
            for (int i = 0; i < materials.Length; i++) {
                materials[i].color = Color.red;
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            materials = item.GetComponent<Renderer>().materials;
            for (int i = 0; i < materials.Length; i++) {
                materials[i].color = originalColors[i];
            }
        }
    }
}
