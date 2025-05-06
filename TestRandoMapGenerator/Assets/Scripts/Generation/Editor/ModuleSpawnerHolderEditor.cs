using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModuleSpawnerHolder))]
public class ModuleSpawnerHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ModuleSpawnerHolder mod = (ModuleSpawnerHolder)target;

        if (GUILayout.Button("GetDeckungSpawner"))
        {
            mod.GetDeckungSpawner();
        }
        if (GUILayout.Button("GetEnemySpawner"))
        {
            mod.GetEnemySpawner();
        }

    }
}
