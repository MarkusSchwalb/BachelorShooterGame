using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;

[CustomEditor(typeof(AdvancedLevelGenerator))]
public class AdvancedLevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AdvancedLevelGenerator generator = (AdvancedLevelGenerator)target;

        if (GUILayout.Button("GenerateMap"))
        {
            generator.GenerateLevel();
        }
        if (GUILayout.Button("Intensity Curve"))
        {
            generator.GenerateIntensityCurve();
        }
        
    }
}
