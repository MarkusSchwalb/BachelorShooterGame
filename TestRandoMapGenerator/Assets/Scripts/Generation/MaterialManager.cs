using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialManager : MonoBehaviour
{

    public static MaterialManager Instance { get; private set; }

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if (Application.isPlaying) 
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject); // Falls schon eine Instanz existiert, zerstören
        }
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject); // Falls schon eine Instanz existiert, zerstören
        }
    }

    [Header("Materialien")]
    public Material DefaultMaterial;

    public Material[] WallMats;
    public Material[] FloorMats;

    public Material GetMaterial(MaterialType type, int nr)
    {
        int i = 0;
        switch (type)
        {
            case MaterialType.Wall:
                i = nr % WallMats.Length;
                //Debug.Log(i);
                return WallMats[i];
            case MaterialType.Floor:
                i = nr % FloorMats.Length;
                //Debug.Log(i);
                return FloorMats[i];
            default: return DefaultMaterial;
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && Instance == this)
        {
            Instance = null; // Löscht die Instanz, wenn das GameObject zerstört wird
        }
    }

}

public enum MaterialType
{
    Wall,
    Floor
}
