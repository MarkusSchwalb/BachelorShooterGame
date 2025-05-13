using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple Inventorys");
        }
        Instance = this;
    }
    #endregion

    public delegate void OnXPChanged(int xpChange);
    public OnXPChanged onXPChanged;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
