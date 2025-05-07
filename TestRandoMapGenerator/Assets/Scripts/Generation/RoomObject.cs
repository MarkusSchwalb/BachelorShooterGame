using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [Header("Exits")]
    public Transform[] Exits;
    public Exit[] EExit;

    [Header("Transform where the objects and not the exits are")]
    public Transform[] Constructs;

    [Header("Objects that can be changed materials")]
    public List<ChangeMaterial> ChangeMatsWall;
    public List<ChangeMaterial> ChangeMatsFloor;

    [Header("Available Exits (for code only could also be private will be changed)" )]
    public List<Exit> ExitList = new List<Exit>();

    [Header("Modules")]
    public Modules[] RoomModules;

    [Header("Spawners")]
    public List<EnemySpawner> ESpawners = new List<EnemySpawner>();
    public List<DeckungSpawner> DSpawners = new List<DeckungSpawner>();
    
    //public ModulManager[] MManagers;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckExits()
    {
        ExitList.Clear();
        int i = 0;
        foreach (Exit exit in EExit) 
        {
            //Debug.Log(i);
            i++;
            if (exit.CheckAvailable())
            {
                ExitList.Add(exit);
            }
        }
        
        if (ExitList.Count == 0)
        {
            Debug.LogWarning("No decent Exit found for some purposes we take number one exit" + gameObject.name);
            if (EExit[0] == null) Debug.LogError(name + " Seems to have no Exit");
            else
            ExitList.Add(EExit[0]);
        }
    }

    internal void DeleteDirection(direction value)
    {
        //Debug.LogWarning("DeleteDirection: " + value);
        if (ExitList.Count < 2) { return; }

        //ExitList.RemoveAll(exit => exit.ExitDirection == value);

        for (int i = ExitList.Count - 1; i >= 0; i--)
        {
            if (ExitList[i].ExitDirection == value)
            {
                ExitList.RemoveAt(i);
            }
        }
    }

    public void ResetExitBoolIsMainPath()
    {
        foreach (Exit exit in EExit) 
        {
            exit.SetIsMainPath(false);
        }
    }

    public void FinalizeRoom()
    {
        Debug.Log("FinalizeRoom");

        HandleOpenExits();
        //SpawnEnemys();  //Maybe later more of a handle spawner (spawner as main chategory and enemy spawner ammunition spawner and stuff as a under chategory) //SpawnEnemys muss anscheinend nach bake maps passieren

        // Spawn Modules
        FinalizeModules();

        // Spawn Deckungen
        SpawnDeckung();

        //Change Material
        GetChangeMats();
        ChangeMaterialsOfMainStuff();
    }

    private void FinalizeModules()
    {
        foreach (Modules module in RoomModules)
        {
            if (module.Room == null) { module.Room = this; }
            module.DeleteChildren();
            module.SpawnModules();
        }
    }

    private void ChangeMaterialsOfMainStuff()
    {
        Debug.Log("ChangeMaterialsOfMainStuff");
        int randomNr = UnityEngine.Random.Range(0, 10);
        foreach (ChangeMaterial material in ChangeMatsWall) {
            material.ChangeMat(randomNr);
        }
        randomNr = UnityEngine.Random.Range(0, 10);
        foreach (ChangeMaterial material in ChangeMatsFloor)
        {
            material.ChangeMat(randomNr);
        }
    }

    private void SpawnDeckung()
    {
        DSpawners.RemoveAll(item => item == null);
        foreach (DeckungSpawner spawner in DSpawners)
        {
            spawner.SpawnDeckung();
        }
    }

    public void SpawnEnemys()
    {
        ESpawners.RemoveAll(item => item == null);
        if (ESpawners.Count == 0) { Debug.LogWarning("Room " + gameObject.name + " has no EnemySpawner be awere if its Start or endroom it is perfectly fine"); return; }

        foreach (EnemySpawner spawner in ESpawners)
        {
            spawner.SpawnEnemy();
        }
    }

    private void HandleOpenExits()
    {
        foreach (Exit exit in EExit)
        {
            if (!exit.IsMainPath)
            {
                exit.HandleEndOfPath();
            }
            if (exit.IsMainPath)
            {
                exit.DeleteChildren();
            }
        }
    }

    public void GetChangeMats()
    {
        Debug.Log("MaterialChange in Room: " + name);
        ChangeMatsWall.Clear();
        ChangeMatsFloor.Clear();

        foreach (Transform t in Constructs)
        {
            recursiveChangeMats(t);
        }
        
    }

    void recursiveChangeMats(Transform parent)
    {
        
        //int i = 0;
        foreach (Transform child in parent)
        {
            //Debug.Log(i);
            //i++;
            if (child.gameObject.TryGetComponent<ChangeMaterial>(out ChangeMaterial material))
            {
                if (child.gameObject.CompareTag("Floor")) ChangeMatsFloor.Add(material);
                if (child.gameObject.CompareTag("Wall")) ChangeMatsWall.Add(material);

                //Debug.Log("change Material gefunden auf: " + child.gameObject.name);

            }
            if (child.gameObject.TryGetComponent<Exit>(out Exit exit)) // Bei Exits nicht weiter machen weil drunter ist nächester raum
            {
                continue;
            }

            recursiveChangeMats(child);
        }
    }
}
