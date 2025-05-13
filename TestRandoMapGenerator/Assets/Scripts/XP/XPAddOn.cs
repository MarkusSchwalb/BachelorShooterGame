using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPAddOn : MonoBehaviour
{
    [SerializeField] private int xPAmount = 100;

    XPManager manager;

    //private HealthComponent healthComponent;

    // Start is called before the first frame update
    void Start()
    {
        //healthComponent = GetComponent<HealthComponent>();
        manager = XPManager.Instance;
        //if (healthComponent == null) { return; }

        //healthComponent.Simple += GainXP;
    }

    private void GainXP()
    {
        Debug.Log(name + "gave" + xPAmount);
        manager.GainXP(xPAmount);
    }

    
}
