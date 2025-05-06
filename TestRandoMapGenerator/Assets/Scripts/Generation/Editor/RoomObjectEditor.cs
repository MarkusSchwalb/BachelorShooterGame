using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomObject))]
public class RoomObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RoomObject roomObject = (RoomObject)target;

        if (GUILayout.Button("FinilazeRoom"))
        {
            roomObject.FinalizeRoom();
        }
        if (GUILayout.Button("GetChangeMaterialChildren"))
        {
            roomObject.GetChangeMats();
        }
    }
}
