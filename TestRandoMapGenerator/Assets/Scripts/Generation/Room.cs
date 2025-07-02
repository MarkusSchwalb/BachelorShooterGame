using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoom", menuName = "Rooms/StandardRoom")]
public class Room : ScriptableObject
{
    public string RoomName;
    public GameObject RoomObject;

    public int Difficulty;

    public int Threat;
    public int Tempo;
    public int Tension;
    public int MovementImpetus;

    public RoomType RoomType;
}

public enum RoomType
{
    NotDefined,
    Movement,
    Combat,
    Roaming,
    Puzzle,
    notRelevant
}