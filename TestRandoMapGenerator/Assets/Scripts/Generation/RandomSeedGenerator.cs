using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomSeedGenerator : MonoBehaviour
{

    [field: SerializeField] private string[] sSubject; //s for string
    [field: SerializeField] private string[] sVerb;
    [field: SerializeField] private string[] sObject;

    [field: SerializeField] TMP_InputField targetField;

    
    public void GenerateARandomSeed()
    {
        targetField.text = "";

        Debug.Log("RandomSeedGeneration");

        string s = GenerateSentence();

        targetField.text = s;        
    }

    string GenerateSentence()
    {
        // a normal english sentence has the following structure Subject + Verb + Object

        string s = "";
        s += sSubject[UnityEngine.Random.Range(0, sSubject.Length)] + " ";
        s += sVerb[UnityEngine.Random.Range(0, sVerb.Length)] + " ";
        s += sObject[UnityEngine.Random.Range(0, sObject.Length)] + ". ";
        Debug.Log(s);
        return s;
    }
}
