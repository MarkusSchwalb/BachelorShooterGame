using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridPlacer))]
public class GridPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GridPlacer mod = (GridPlacer)target;

        if (GUILayout.Button("DoYourThing"))
        {
            mod.DoProcedural();
        }
        if (GUILayout.Button("Delete"))
        {
            mod.DeleteChildren();
        }
    }
}
