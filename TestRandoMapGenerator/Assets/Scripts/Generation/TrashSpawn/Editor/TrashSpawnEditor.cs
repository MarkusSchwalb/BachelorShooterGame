using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrashSpawn))]
public class TrashSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TrashSpawn deckungSpawner = (TrashSpawn)target;

        if (GUILayout.Button("SpawnTrash"))
        {
            deckungSpawner.SpawnTrash();
        }
        
    }
}
