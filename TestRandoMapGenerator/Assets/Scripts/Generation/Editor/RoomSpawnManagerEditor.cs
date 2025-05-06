using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomSpawnManager))]
public class RoomSpawnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RoomSpawnManager roomSpawnManager = (RoomSpawnManager)target;

        if (GUILayout.Button("GenerateMap"))
        {
            roomSpawnManager.GenerateLevel();
        }
        if (GUILayout.Button("NewSeed+GenerateMap"))
        {
            roomSpawnManager.GenerateNewSeed();
            roomSpawnManager.GenerateLevel();
        }
        if (GUILayout.Button("ClearLevel"))
        {
            roomSpawnManager.ClearLevel();
        }
        if (GUILayout.Button("StepForStep"))
        {
            roomSpawnManager.StepForStep();
        }
        if (GUILayout.Button("RandomSeed"))
        {
            roomSpawnManager.GenerateNewSeed();
        }
    }
}
