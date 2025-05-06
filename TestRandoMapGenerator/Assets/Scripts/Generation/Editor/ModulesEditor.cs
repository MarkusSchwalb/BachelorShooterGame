using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Modules))]
public class ModulesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Modules mod = (Modules)target;

        if (GUILayout.Button("SpawnModules"))
        {
            mod.SpawnModules();
        }
        if (GUILayout.Button("Reset"))
        {
            mod.DeleteChildren();
        }
    }
}
