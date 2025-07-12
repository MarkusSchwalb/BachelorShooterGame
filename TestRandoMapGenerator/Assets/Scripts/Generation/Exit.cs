using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    BoxCollider CheckCollider;
    public LayerMask LayerMask;
    [field: SerializeField] public bool IsMainPath {  get; private set; }
    [field: SerializeField] public bool IsSidePath { get; private set; }

    bool isAvailable = true;
    
    public direction ExitDirection = direction.north;

    public GameObject EndOfPath;

    [field: SerializeField] private RoomObject room;

    // Start is called before the first frame update
    void Start()
    {
        if (room == null) { FindRoomObjcet(); }
        CheckCollider = GetComponent<BoxCollider>();
        if (CheckCollider == null)
        {
            Debug.LogError("No Collider Found for Exit : " + gameObject.name + " of Parent " + gameObject.transform.parent.name);
        }
    }

    private void FindRoomObjcet()
    {
        GameObject parent = transform.parent.gameObject;
        int breaker = 0;
        while (room == null && breaker <= 10)
        {
            if (parent.TryGetComponent<RoomObject>(out RoomObject rO))
            {
                room = rO;
                return;
            }
            parent = parent.transform.parent.gameObject;
            breaker++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckAvailable()
    {
        if ( IsSidePath || IsMainPath)
        {
            return false;
        }
        if (LayerMask == 0) { LayerMask = LayerMask.GetMask("Rooms"); }
        //Debug.Log("Check Exit Availibility of " + gameObject.name);
        CheckCollider = GetComponent<BoxCollider>();
        if (CheckCollider == null) { Debug.LogError("No Collider Found"); return false; }

        //if (isAvailable) { return true; }

        if (CheckSpace())
        {
            isAvailable = true;
            //Debug.Log("Exit check returns true");
            return true;
        }

        isAvailable = false;
        //Debug.Log("Exit check returns false");
        return false;
    }

    private bool CheckSpace()
    {
        Collider[] colliders = Physics.OverlapBox(
            CheckCollider.bounds.center, // Mittelpunkt des Colliders
            CheckCollider.bounds.extents, // Größe des Colliders
            transform.rotation, // Rotation des Colliders
            LayerMask
        );

        int maxColliders = 1; //make sure the room object doesn't count into it

        if (room != null) 
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == room.gameObject)
                {
                    maxColliders++;
                }
            }
        }

        //Debug.Log("collider count: " + colliders.Length);
        
        if (colliders.Length > maxColliders) { 
            Debug.Log(gameObject.name + "Ist versperrt");
            foreach (Collider collider in colliders)
            {
                Debug.Log(collider.name);
            }
            return false; 
        } //more than one collider = exit blocked (Check collider ist auch ein collider deshalb ist immer einer drin)

        return true; //no collider = exit free
    }

    private void OnCollisionEnter(Collision collision)
    {
        isAvailable = false;
    }
    
    private void OnDrawGizmos()
    {
        BoxCollider MyCollider = GetComponent<BoxCollider>();

        if (MyCollider == null) return;

        // Liste aller überlappenden Collider abrufen
        Collider[] colliders = Physics.OverlapBox(
            MyCollider.bounds.center, // Mittelpunkt des Colliders
            MyCollider.bounds.extents, // Größe des Colliders
            transform.rotation, // Rotation des Colliders
            LayerMask
        );

        // Zeichne Gizmos, um Kollisionen sichtbar zu machen
        Gizmos.color = colliders.Length > 1 ? Color.red : Color.green; // Rot, wenn es eine Kollision gibt
        Gizmos.DrawWireCube(MyCollider.bounds.center, MyCollider.bounds.size);
    }

    public void SetIsMainPath(bool value)
    {
        //Debug.Log("SetIsMainPath value: " + value);
        IsMainPath = value;
    }

    public void HandleEndOfPath()
    {

        /*
        if (IsMainPath) { return; }
        Instantiate(EndOfPath, transform); // maybe later with a check Space if sideroom*/
    }

    public void DeleteChildren()
    {
        if (gameObject.transform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }

    internal void SetIsSidePath(bool v)
    {
        IsSidePath = v;
    }
}

public enum direction
{
    north, east , south, west, NotDefined
}
