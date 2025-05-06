using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSplitter : MonoBehaviour
{
    public Modules[] Modules;

   
    public void FinalizeModules()
    {
        Debug.Log("ModuleSplitter Finalize Module");
        foreach (Modules module in Modules)
        {
            module.SpawnModules();
        }
    }
}
