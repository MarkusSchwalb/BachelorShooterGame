using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    //public Material newWallMaterial; // Neues Material im Inspector zuweisen
    private Material newWallMaterial;
    internal void ChangeMat(int randomNr)
    {
        //Debug.Log("ChangeMat");
        
        if (MaterialManager.Instance == null) { Debug.LogWarning("No Material Manager"); return; }
        
        if (gameObject.CompareTag("Wall")) { newWallMaterial = MaterialManager.Instance.GetMaterial(MaterialType.Wall, randomNr); }
        if (gameObject.CompareTag("Floor")) { newWallMaterial = MaterialManager.Instance.GetMaterial(MaterialType.Floor, randomNr); }

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = newWallMaterial;
        } else { Debug.LogError("Found No Renderer"); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
