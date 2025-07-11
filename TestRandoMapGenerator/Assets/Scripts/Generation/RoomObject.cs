using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [Header("Exits")]
    public Transform[] Exits;
    public Exit[] EExit;

    public Transform nextMainExit {  get; private set; }
    public Transform[] potentialReward { get; private set; }

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

    [Header("Trash")]
    [field: SerializeField] private List<TrashSpawn> trashSpawns = new List<TrashSpawn>();
    [field: SerializeField] private float trashiness = 0.5f;
    [field: SerializeField] public TrashOptions trashOptions;

    [field: SerializeField] public int Intensity { get; set; } = 0;

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
            if (exit == null) continue;
            //Debug.Log(i);
            i++;
            if (exit.CheckAvailable())
            {
                ExitList.Add(exit);
            }
        }
        
        if (ExitList.Count == 0)
        {
            Debug.Log("No decent Exit found for some purposes we take number one exit" + gameObject.name);
            if (EExit == null || EExit.Length == 0 || EExit[0] == null) Debug.Log(name + " Seems to have no Exit");
            else
            ExitList.Add(GetDirectionExit(direction.north));
        }
    }

    /*
    public int GetAvailableSideExit()
    {
        List<int> sideExit = new List<int>();
        for (int i = 0; i < ExitList.Count; i++)
        { 
            if (!ExitList[i].IsMainPath && ExitList[i].CheckAvailable())
            {

            }
        }

        return 99; //Error return
    }*/

    private Exit GetDirectionExit( direction face)
    {
        int i = 0;
        foreach (Exit exit in EExit)
        {
            //Debug.Log(i);
            i++;
            if (exit.ExitDirection == face) return exit;
        }

        return EExit[0];
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

    public void ResetExitBoolIsSidePath()
    {
        foreach (Exit exit in EExit)
        {
            exit.SetIsSidePath(false);
        }
    }

    public void FinalizeRoom()
    {
        //Debug.Log("FinalizeRoom");

        //HandleOpenExits(); //darüber denken wir noch
        //SpawnEnemys();  //Maybe later more of a handle spawner (spawner as main chategory and enemy spawner ammunition spawner and stuff as a under chategory) //SpawnEnemys muss anscheinend nach bake maps passieren

        // Spawn Modules
        FinalizeModules();

        // Spawn Deckungen
        SpawnDeckung();

        //Change Material
        GetChangeMats();
        ChangeMaterialsOfMainStuff();

        //spawn trash
        SpawnTrash();
    }

    public void FinalizeRoom(int intensity)
    {
        Intensity = intensity;

        FinalizeRoom();

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

    public void ChangeMaterialsOfMainStuff()
    {
        //Debug.Log("ChangeMaterialsOfMainStuff");
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
        //Debug.Log("MaterialChange in Room: " + name);
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

    private void SpawnTrash()
    {
        /*Debug.Log("SpawnTrash");
        GetTrashSpawners();
        

        foreach (TrashSpawn spawn in trashSpawns)
        {
            spawn.SpawnTrash(trashiness, this);
        }*/
        
    }

    private void GetTrashSpawners()
    {
        trashSpawns.Clear();

        foreach (Transform child in Constructs)
        {
            if (child.gameObject.TryGetComponent<TrashSpawn>(out TrashSpawn spawn))
            {
                trashSpawns.Add(spawn);
            }

            if (child.gameObject.TryGetComponent<Exit>(out Exit exit)) // Bei Exits nicht weiter machen weil drunter ist nächester raum
            {
                continue;
            }

            RecursiveGetTrash(child);
        }
    }

    private void RecursiveGetTrash(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent<TrashSpawn>(out TrashSpawn spawn))
            {
                trashSpawns.Add(spawn);
            }

            if (child.gameObject.TryGetComponent<Exit>(out Exit exit)) // Bei Exits nicht weiter machen weil drunter ist nächester raum
            {
                continue;
            }

            RecursiveGetTrash(child);
        }
    }
    /*
    internal void CalculateExits(List<direction> notAvailable)
    {
        ResetExitBoolIsMainPath();
        ResetExitBoolIsSidePath();
        CheckExits();
        
        if (ExitList.Count <= 1)
        {
            nextMainExit = ExitList[0].transform;

        }

        if (notAvailable.Count >= 3)
        {
            //choose North
            Exit e = ExitList.Find(exit => exit.ExitDirection == direction.north);
            e.SetIsMainPath(true);
        }
        List<Exit> potentialMainPath = new List<Exit>();
        foreach (Exit exit in ExitList)
        {
            if (!notAvailable.Contains(exit.ExitDirection))
            {
                potentialMainPath.Add(exit);
            }
        }
        if (potentialMainPath.Count < 1)
        {
            //choose North
            Exit e = ExitList.Find(exit => exit.ExitDirection == direction.north);
            e.SetIsMainPath(true);
        }

        int rInt =


    }*/

    

    public bool CheckStuckInOtherRoom()
    {
         LayerMask mask = LayerMask.GetMask("Rooms"); 
        //Debug.Log("Check Exit Availibility of " + gameObject.name);
        BoxCollider checkCollider = GetComponent<BoxCollider>();
        if (checkCollider == null) { Debug.LogError("No Collider Found"); return false; }

        Collider[] colliders = Physics.OverlapBox(
            checkCollider.bounds.center, // Mittelpunkt des Colliders
            checkCollider.bounds.extents, // Größe des Colliders
            transform.rotation, // Rotation des Colliders
            mask
        );

        int roomCount = 0;
        foreach (Collider col in colliders)
        {
            if (col.gameObject == gameObject) continue;
            if (CheckIfParentRoom(col.gameObject) || CheckIfDirectChildRoom(col.gameObject)) continue;
            if (col.gameObject.TryGetComponent<RoomObject>(out RoomObject room))
            {
                roomCount++;
                Debug.Log(gameObject.name + " Rooms Collided with " + col.gameObject.name);
            }
        }

        if (roomCount > 0)
        {
           
            foreach (Collider collider in colliders)
            {
                Debug.Log(collider.name);
            }
            return false;
        }

        //Debug.Log("Exit check returns false");
        return true;
    }

    private bool CheckIfParentRoom(GameObject gO)
    {
        GameObject parent = transform.parent.gameObject;
        for (int i = 0; i < 5 ; i++)
        {
            if (parent == gO) return true;
            parent = parent.transform.parent.gameObject;
        }
        return false;
    }

    private bool CheckIfDirectChildRoom(GameObject gO)
    {
        if (EExit == null) return false;
        foreach (Exit exit in EExit)
        {
            foreach (Transform t in exit.transform)
            {
                if (t.gameObject == gO)
                {
                    return true;
                }
            }
        }
        return false;
    }

    internal void CheckExitsforRewards()
    {
        if (ExitList == null)
        {
            Debug.Log(gameObject.name + " Has exits that have no Exit list");
            return;
        }
        ExitList.Clear();
        int i = 0;
        foreach (Exit exit in EExit)
        {
            if (exit == null) continue;

            //Debug.Log(i);
            i++;
            if (exit.CheckAvailable())
            {
                ExitList.Add(exit);
            }
        }
    }
}
