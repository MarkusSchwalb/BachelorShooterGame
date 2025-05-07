using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LabyrinthBrain))]
public class LabyrinthBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LabyrinthBrain mod = (LabyrinthBrain)target;

        if (GUILayout.Button("DoProcedural"))
        {
            mod.DoProcedural();
        }
        
    }
}
