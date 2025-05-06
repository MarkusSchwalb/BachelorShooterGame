using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeckungSpawner))]
public class DeckungSpawnManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DeckungSpawner deckungSpawner = (DeckungSpawner)target;

        if (GUILayout.Button("SpawnCover"))
        {
            deckungSpawner.SpawnDeckung();
        }
        if (GUILayout.Button("Reset"))
        {
            deckungSpawner.ResetCovers();
        }
        if (GUILayout.Button("GetDeckungen"))
        {
            deckungSpawner.GetDeckungen();
        }
    }
}
