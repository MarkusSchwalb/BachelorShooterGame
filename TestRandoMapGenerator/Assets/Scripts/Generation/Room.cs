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

    [Tooltip("the high the more likely its going to be chosen 10 is the standard Value")]
    public int Commoness = 10;

    public RoomType RoomType;
}

public enum RoomType
{
    NotDefined,
    Movement,
    Combat,
    Roaming,
    Puzzle,
    Reward,
    notRelevant
}