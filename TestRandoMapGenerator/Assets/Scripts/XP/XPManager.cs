using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    #region Singleton
    public static XPManager Instance;

    public delegate void OnXPChanged(int xpChange);
    public OnXPChanged onXPChanged;

    public GameObject currentXPObject {  get; private set; }

    

    [field: SerializeField]
    public int currentXP {  get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple Inventorys");
        }
        Instance = this;
        /*
        PlayerPrefs.SetInt("Score", 9999);
        PlayerPrefs.Save();

        int testLoad = PlayerPrefs.GetInt("Score", -1);
        Debug.Log("XPMANAGER Test direkt nach Save geladen: " + testLoad);*/
    }
    #endregion

    private void Start()
    {
        Debug.Log("XP MANAGER Start");
        LoadXP();
    }

    public void GainXP(int xpGain)
    {
        Debug.Log($"GainXP {xpGain}");
        if (xpGain < 1) { return; }
        currentXP += xpGain;

        if (onXPChanged != null)
        {
            onXPChanged?.Invoke(xpGain);
        }
        SaveXP();
    }


    public void LoadXP()
    {
        Debug.Log("XPLoaded " + PlayerPrefs.GetInt("Score", 0));
        currentXP = PlayerPrefs.GetInt("Score", 0); 
        onXPChanged?.Invoke(currentXP);
        SaveXP();

        if (onXPChanged != null)
        {
            onXPChanged?.Invoke(currentXP);
        }
    }
    public void SaveXP()
    {
        Debug.Log("xpGespeichert");
        GameData.Score = currentXP;
        PlayerPrefs.SetInt("Score", currentXP);
        PlayerPrefs.Save();
    }

    public void SetCurrentXPObject(GameObject newXpObject)
    {
        Debug.Log("SetCurrentXPObject");
        currentXPObject = newXpObject;
        onXPChanged?.Invoke(currentXP);
    }

    public bool ReduceXP(int reduceValue)
    {
        Debug.Log("ReduceXP");
        if (reduceValue > currentXP) { return false; }
        currentXP -= reduceValue;
        onXPChanged?.Invoke(-reduceValue);
        SaveXP() ;
        return true;
    }

    private void OnDestroy()
    {
        Debug.Log("ReduceXP");
        SaveXP();
    }

    private void Update()
    {
        Debug.Log("xp" + currentXP);
    }

    internal void Subscribed()
    {
        onXPChanged?.Invoke(currentXP);
    }
}
