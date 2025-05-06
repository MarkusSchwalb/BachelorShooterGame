using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Modules : MonoBehaviour
{
    public RoomObject Room;
    public ProceduralModulLogicBase ModulLogic;
    public Transform ModuleTransform;

    public GameObject[] ModulesParts;
    [Tooltip("If not exclusive than it can spawn Multiple Modules")]
    [field: SerializeField] private bool isExclusive = true;
    public List<int> spawnedModules = new List<int>();
    [Tooltip("GameObject=GO")]
    public List<GameObject> spawnedModulesGO = new List<GameObject>();
    
    public void SpawnModules()
    {
        if (ModulLogic != null) { 
            ModulLogic = GetComponent<ProceduralModulLogicBase>();
            ModulLogic.DoProcedural(); 
            return; }

        if (spawnedModulesGO==null) spawnedModulesGO = new List<GameObject>();

        if (ModulesParts.Length == 0) { Debug.LogError("Module on " + gameObject.name + "Has no Parts"); return; }

        DeleteChildren();

        if (isExclusive) SpawnExclusivePart();
        if (!isExclusive) SpawnNonExclusive();
    }

    public void DeleteChildren()
    {
        DeleteFromLists();
        
        if (gameObject.transform.childCount != 0)
        {
            if (ModuleTransform == null) { Debug.LogWarning("please make sure you have a parent Transform for the Modules" + name); }

            for (int i = ModuleTransform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(ModuleTransform.GetChild(i).gameObject);
            }
        }
    }

    private void DeleteFromLists()
    {
        if (Room == null)
        { 
            if (transform.parent.TryGetComponent<RoomObject>(out RoomObject roomObject))
            {
                Room = roomObject;
            } else
            {
                Debug.LogError(gameObject.name + "Modul has no assigned RoomObject");
                spawnedModules.Clear();
                spawnedModulesGO.Clear();
                return;
            }
        }

        spawnedModulesGO.RemoveAll(item => item == null);

        if (spawnedModulesGO.Count == 0) { return; }

        foreach (GameObject gO in spawnedModulesGO)
        {
            
            if (gO.TryGetComponent<ModuleSpawnerHolder>(out ModuleSpawnerHolder holder))
            {
                //delete from room object those covers and Enemy Spawner
                foreach (EnemySpawner enemySpawner in holder.EnemySpawners)
                {
                    Room.ESpawners.Remove(enemySpawner);
                }
                foreach (DeckungSpawner deckungSpawner in holder.DeckungSpawnerList)
                {
                    Room.DSpawners.Remove(deckungSpawner);
                }

            }
        }
        spawnedModules.Clear();
        spawnedModulesGO.Clear();
    }

    private void SpawnNonExclusive()
    {
        int anzlModules = UnityEngine.Random.Range(0, ModulesParts.Length);

        for (int i = 0; i < anzlModules; i++)
        {
            int randomModInt = UnityEngine.Random.Range(0, anzlModules);

            while (spawnedModules.Contains(randomModInt))  //make sure we dont have Multiples
            {
                randomModInt++;
                randomModInt = randomModInt % ModulesParts.Length;
            }

            GameObject SpawnObject = Instantiate(ModulesParts[randomModInt], ModuleTransform); //Spawn
            CheckForModule(SpawnObject);
            spawnedModulesGO.Add(SpawnObject);
            spawnedModules.Add(randomModInt);
        }

        UpdateLists();
    }

    private void CheckForModule(GameObject module)
    {
        Modules[] moudlesOnModule = module.GetComponents<Modules>();
        Debug.Log("Check for Modules | Number Found Modules: " + moudlesOnModule.Length);
        foreach (Modules mod in moudlesOnModule)
        {
            mod.SpawnModules();

            if (mod.Room == null && Room != null)
            {
                mod.Room = Room;
            }
        }

        if (module.TryGetComponent<ModuleSplitter>(out ModuleSplitter splitter))
        {
            splitter.FinalizeModules();
        }

        /*
        if (module.TryGetComponent<Modules>(out Modules moduleComp))
        {
            moduleComp.SpawnModules();
            if (moduleComp.Room == null && Room != null)
            {
                moduleComp.Room = Room;
            }
        }*/
    }

    /*
     Colliders[] colliders = gO.GetComponents<Modules>();

        foreach (Colliders col in colliders)
        {
            //do something
        }
     */

    private void SpawnExclusivePart()
    {
        int randomInt = UnityEngine.Random.Range(0, ModulesParts.Length);
        GameObject SpawnObject;
        if (ModuleTransform != null)
        {
            if (ModulesParts[randomInt]==null) { Debug.LogError(name + " Has a unassigned Module Part at " + randomInt); return; }
            SpawnObject = Instantiate(ModulesParts[randomInt], ModuleTransform);
        }
        else SpawnObject = Instantiate(ModulesParts[randomInt], gameObject.transform);

        spawnedModulesGO.Add(SpawnObject);
        spawnedModules.Add(randomInt);
        CheckForModule(SpawnObject);

        UpdateLists();
    }

    private void UpdateLists()
    {
        foreach (GameObject gO in spawnedModulesGO)
        {

            if (gO.TryGetComponent<ModuleSpawnerHolder>(out ModuleSpawnerHolder holder))
            {
                //delete from room object those covers and Enemy Spawner
                foreach (EnemySpawner enemySpawner in holder.EnemySpawners)
                {
                    Room.ESpawners.Add(enemySpawner);
                }
                foreach (DeckungSpawner deckungSpawner in holder.DeckungSpawnerList)
                {
                    Room.DSpawners.Add(deckungSpawner);
                }

            }
        }
    }
}
