using System;
using UnityEngine;

public class PlayerEntersTriigger : MonoBehaviour
{
    public RoomObject RoomObj;
    private void Start()
    {
        if (RoomObj == null)
            FindParentRoom();
    }

    private void FindParentRoom()
    {
        GameObject parent = transform.parent.gameObject;
        for (int i = 0; i < 3; i++)
        {
            if(parent.TryGetComponent<RoomObject>(out RoomObject rO))
            {
                RoomObj = rO;
                return;
            }
            parent = parent.transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered " + other.name);
        if (RoomObj == null) return;
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerEnteredTrigger " + other.name);
            if (RoomObj == null) Debug.Log("HasNoRoom");
            RoomObj.SpawnEnemys();
            //you could probably save the room here
            GameData.LastRoom = RoomObj.name;
        }
    }
}
