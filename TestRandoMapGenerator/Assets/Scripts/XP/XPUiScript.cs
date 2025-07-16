using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class XPUiScript : MonoBehaviour
{
    XPManager xPManager;
    
    public TextMeshProUGUI CurrentXPField;

    public GameObject GainedXPPrefab;
    public Transform GainedXPHolder;

    //public TextMeshProUGUI GainedXPField;

    //public GameObject GainedUiSlot;

    private bool showGainedUI;

    //[SerializeField] private float showTime = 5;
    private float counter;
    private List<GameObject> GainedUIObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        xPManager = XPManager.Instance;
        xPManager.onXPChanged += UpdateUI;
        xPManager.Subscribed();
    }

    private void UpdateUI(int xpChange)
    {
        if (CurrentXPField == null) return;

        //make shure the GainedXp Field is shown
        //showGainedUI = true;
        counter = 0;
        //GainedUiSlot.SetActive(true);

        //update Text
        CurrentXPField.text = xPManager.currentXP.ToString();

        //Limit Amount of Gained Ui
        GainedUIObjects.RemoveAll(item => item == null);
        
        if (GainedUIObjects.Count > 3)
        {
            Destroy(GainedUIObjects[0]);
        }

        //SpawnNewGainedXp Field and write gained amount
        GameObject newGainedField = Instantiate(GainedXPPrefab, GainedXPHolder);

        if(newGainedField.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmpxGui))
        {
            tmpxGui.text = xpChange.ToString();
        }

        GainedUIObjects.Add(newGainedField);
        Destroy(newGainedField, 0.2f);
    }

    private void Update()
    {
        /*
        //disable the gained slot after time
        if (!showGainedUI) { return; }
        counter += Time.deltaTime;

        if (counter > showTime) 
        { 
            showGainedUI = false;
            GainedUiSlot.SetActive(false);
        }*/
    }


}
