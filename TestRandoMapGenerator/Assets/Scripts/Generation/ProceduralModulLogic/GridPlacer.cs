using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class GridPlacer : ProceduralModulLogicBase
{
    [Header("This script is derived from ProceduralModulLogicBase")]
    public GameObject[] PlaceableObject;
    public RoomObject roomObject;

    public bool IsRandomAmount = true;
    public bool IsReliableGrid = true;
    [Header("Reliability in percentage")]
    public float Reliability = 100;

    [Header("Metrics")]
    public int Maxforward = 1;
    public int MaxRight = 1;
    
    public int Minforward = 1;
    public int MinRight = 1;

    public float ForwardSpacing = 1;
    public float RightSpacing = 1;
    public override void DoProcedural()
    {
        DeleteChildren();
        GetRoomObject();

        int targetForward;
        int targetRight;
        if (IsRandomAmount)
        {
            targetForward = UnityEngine.Random.Range(Minforward, Maxforward + 1);
            targetRight = UnityEngine.Random.Range(MinRight, MaxRight + 1);
        }
        else
        {
            targetForward = Maxforward;
            targetRight = MaxRight;
        }
        
        if (IsReliableGrid)
        {
            SpawnReliable(targetForward, targetRight);
        }
        else
        {
            SpawnUnreliable(targetForward, targetRight);
        }
        
    }

    private void GetRoomObject()
    {
        if (roomObject != null) { return; }


        Modules mod = GetComponent<Modules>();
        if (mod == null) { return; }
        if (mod.Room != null) { roomObject = mod.Room; }
    }

    private void SpawnUnreliable(int targetForward, int targetRight)
    {

        for (int i = 0; i < targetForward; i++)
        {
            for (int j = 0; j < targetRight; j++)
            {
                if (!CheckReliability()) continue;
                
                // Local Position 
                Vector3 localPos = new Vector3(j * RightSpacing, 0, i * ForwardSpacing);
                // World Position
                Vector3 worldPos = transform.TransformPoint(localPos);

                int randomModInt = UnityEngine.Random.Range(0, PlaceableObject.Length);
                GameObject spawnObject = Instantiate(
                    PlaceableObject[randomModInt], worldPos,
                    transform.rotation,
                    transform); //Spawn

                CheckForModules(spawnObject);
            }
        }
    }

    private bool CheckReliability()
    {
        float randomNumber = UnityEngine.Random.Range(0, 101);
        if (randomNumber < Reliability) { return true; }

        return false;
    }

    private void SpawnReliable(int targetForward, int targetRight)
    {

        for (int i = 0; i < targetForward; i++)
        {
            for (int j = 0; j < targetRight; j++)
            {
                // Local Position 
                Vector3 localPos = new Vector3(j * RightSpacing, 0, i * ForwardSpacing);
                // World Position
                Vector3 worldPos = transform.TransformPoint(localPos);

                int randomModInt = UnityEngine.Random.Range(0, PlaceableObject.Length);
                GameObject spawnObject = Instantiate(
                    PlaceableObject[randomModInt], worldPos,
                    transform.rotation,
                    transform); //Spawn

                CheckForModules(spawnObject);
            }
        }
    }

    private void CheckForModules(GameObject spawnObject)
    {
        Modules[] moudlesOnModule = spawnObject.GetComponents<Modules>();
        Debug.Log("Check for Modules | Number Found Modules: " + moudlesOnModule.Length);
        foreach (Modules mod in moudlesOnModule)
        {
            mod.SpawnModules();

            if (mod.Room == null && roomObject != null)
            {
                mod.Room = roomObject;
            }
        }

        if (spawnObject.TryGetComponent<ModuleSplitter>(out ModuleSplitter splitter))
        {
            splitter.FinalizeModules();
        }
    }

    public void DeleteChildren()
    {
        if (gameObject.transform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }
}
