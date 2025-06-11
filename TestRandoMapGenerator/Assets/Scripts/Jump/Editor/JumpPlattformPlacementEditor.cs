using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JumpPlattformPlacement))]

public class JumpPlattformPlacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        JumpPlattformPlacement jumpPlattformPlacement = (JumpPlattformPlacement)target;

        if (GUILayout.Button("SpawnJumpPlattforms"))
        {
            jumpPlattformPlacement.DoProcedural();
        }
    }
}
