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
    }
    #endregion

    private void Start()
    {
        ResetXP();
    }

    public void GainXP(int xpGain)
    {
        if (xpGain < 1) { return; }
        currentXP += xpGain;

        if (onXPChanged != null)
        {
            onXPChanged?.Invoke(xpGain);
        }
    }


    public void ResetXP()
    {
        currentXP = 0;
        onXPChanged?.Invoke(-currentXP);
    }

    public void SetCurrentXPObject(GameObject newXpObject)
    {
        currentXPObject = newXpObject;
    }

    public bool ReduceXP(int reduceValue)
    {
        if (reduceValue > currentXP) { return false; }
        currentXP -= reduceValue;
        return true;
    }

    private void Update()
    {
        
    }
}
