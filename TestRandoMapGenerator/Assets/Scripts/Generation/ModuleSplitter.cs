using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSplitter : MonoBehaviour
{
    public RoomObject room;
    public Modules[] Modules;

   
    public void FinalizeModules()
    {
        Debug.Log("ModuleSplitter Finalize Module");
        foreach (Modules module in Modules)
        {
            module.SpawnModules();        
        }
    }

    public void FinalizeModules(RoomObject ro)
    {
        Debug.Log("ModuleSplitter Finalize Module");
        foreach (Modules module in Modules)
        {
            if (module.Room == null) { module.Room = ro; }
            module.SpawnModules();

        }
    }
}
