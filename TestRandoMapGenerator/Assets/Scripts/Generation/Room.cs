using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoom", menuName = "Rooms/StandardRoom")]
public class Room : ScriptableObject
{
    public string RoomName;
    public GameObject RoomObject;

    public Vector3 SizePoint1;
    public Vector3 SizePoint2;

    

    public int Difficulty;

}
