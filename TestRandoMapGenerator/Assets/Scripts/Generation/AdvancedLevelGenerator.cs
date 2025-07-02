using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AdvancedLevelGenerator : MonoBehaviour
{
    [field: SerializeField] public Int32 SeedNmbr { get; private set; }

    [Header("Rooms")]
    [field: SerializeField] public Room StartRoom { get; private set; }
    [field: SerializeField] public Room EndRoom { get; private set; }

    [field: SerializeField] public List<Room> RoomList { get; private set; } = new List<Room>();
    [field: SerializeField] public Room[] InBetweenRooms { get; private set; }

    [field: SerializeField] public Room[] RewardRooms;

    [field: SerializeField] public Room[] ClimaxRooms;

    [Header("Parameter")]
    public bool RandyRandom = false;
    [Tooltip("needs to be even and a number above 6 if you input a uneven number it adds 1 to make it even if " +
        "you make an input below 6 it will be 6")]
    public int MainRoomCount;


    //for intensity curve
    private List<int> intensityCurve = new List<int>();
    public int goalIntensity { get; private set; }

    //Spawned Rooms
    private List<Room> spawnedRooms = new List<Room>();
    private List<RoomObject> spawnedRoomObjects = new List<RoomObject>();
    public RoomObject CurrentRoom { get; private set; }

    private direction lastDirection = direction.NotDefined;
    private int west = 0, east = 0, north = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        ResetSettings();
        CheckAndInitSeed();
        GenerateIntensityCurve();
        StartGeneratingLevel();
        SpawnRooms();
    }

    private void ResetSettings()
    {
        lastDirection = direction.NotDefined;
        
        west = 0; east = 0; north = 0;
        ClearLevel();

        if (!CheckBuildingBlocks()) return;
        CheckBuildingBlocks();

    }

    public void ClearLevel()
    {
        if (transform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }

            spawnedRooms.Clear();
            spawnedRoomObjects.Clear();
        }
    }

    private bool CheckBuildingBlocks()
    {
        if (StartRoom == null) { Debug.LogError("NoStartRoom"); return false; }
        if (EndRoom == null) { Debug.LogError("NoEndRoom"); return false; }
        if (RoomList.Count == 0) { Debug.LogError("NotEnoughInbetweenRooms"); return false; }

        return true;
    }

    public void GenerateNewSeed()
    {
        int newSeed = UnityEngine.Random.Range(0, 2000);
        SeedNmbr = newSeed;
        GameData.Seed = newSeed;
    }

    private void CheckAndInitSeed()
    {
        if (RandyRandom) GenerateNewSeed();
        int seed = GameData.Seed;

        if (seed == 0) 
        { 
            GameData.SetSeed("Ich gebe hier mal was ein");
            seed = GameData.Seed;
        }

        if (GameData.CurrentLevel > 1)
        {
            seed += 10;
        }

        UnityEngine.Random.InitState(seed);
    }

    private void GenerateIntensityCurve()
    {
        if (MainRoomCount < 6) MainRoomCount = 6;
        if (MainRoomCount%2 != 0)
        {
            MainRoomCount++;
        }
        
        intensityCurve.Clear();
        int startIntensity = UnityEngine.Random.Range(1, 2);
        intensityCurve.Add(startIntensity);
        int hookIntensity = UnityEngine.Random.Range(startIntensity + 1, startIntensity + 3);
        
        intensityCurve.Add(hookIntensity);

        int lastIntensity = hookIntensity;
        for (int i = 2; i <= MainRoomCount;  i+=2)
        {
            int newIntensitRest = UnityEngine.Random.Range(lastIntensity - 1, lastIntensity - 2);
            intensityCurve.Add(newIntensitRest);
            lastIntensity = newIntensitRest;
            int newIntensityPeak = UnityEngine.Random.Range(lastIntensity + 2, lastIntensity + 3);
            intensityCurve.Add(newIntensityPeak);
            lastIntensity += newIntensityPeak;
        }
    }
    private void StartGeneratingLevel()
    {
        west = 0; east = 0; north = 0;

        //Spawn Start Room
        GameObject spawnedRoom = Instantiate(StartRoom.RoomObject, transform);
        SetCurrentRoom(spawnedRoom);
        spawnedRooms.Add(StartRoom);
    }

    private void SetCurrentRoom(GameObject spawnedRoom)
    {
        if (spawnedRoom.TryGetComponent<RoomObject>(out RoomObject roomObject))
        {
            CurrentRoom = roomObject;
            spawnedRoomObjects.Add(CurrentRoom);
        }
        else
        {
            Debug.LogError("Spawned Room is no RoomObject" + spawnedRoom.name);
            return;
        }
    }

    
    private void SpawnRooms()
    {
        
        for (int i = 0; i <= MainRoomCount - 1; i++) //minus one because the Climax room is separate
        {
            //finde die aktuelle intensität
            int intens = intensityCurve[i];
            //prüfe die Letzte art von Level
            List<Room> possibleRooms = CheckNextPossibleRooms(i);

        }
    }

    private List<Room> CheckNextPossibleRooms(int i)
    {
        if (i > 9) i = 9;
        List<Room> list = new List<Room>();
        RoomType excludeRoomType = CheckExcludeRoomType();

        foreach (Room room in RoomList)
        {
            if (i - 1 <= room.Difficulty && room.Difficulty <= i + 1)
            {
                if (room.RoomType == excludeRoomType) continue;
                list.Add(room);
            }
        }

        if (list.Count > 0) list = LesserSpecificRoomFilter(i);
        return list;
    }

    private List<Room> LesserSpecificRoomFilter(int i)
    {
        List<Room> list = new List<Room>();
        foreach (Room room in RoomList)
        {
            if (i - 3 <= room.Difficulty && room.Difficulty <= i + 3)
            {
                list.Add(room);
            }
        }
        if (list.Count == 0)
        {
            Room hardest = null;
            Room secondHardest = null;
            Room thirdHardest = null;

            foreach (Room room in RoomList)
            {
                if (hardest == null) hardest = room;
                if (room.Difficulty > hardest.Difficulty)
                {
                    thirdHardest = secondHardest;
                    secondHardest = hardest;
                    hardest = room;
                }
            }
            list = new List<Room> { hardest, secondHardest, thirdHardest };
        }
        return list;
    }

    private RoomType CheckExcludeRoomType()
    {
        if (spawnedRooms[spawnedRooms.Count - 1].RoomType == spawnedRooms[spawnedRooms.Count - 2].RoomType 
            && spawnedRooms[spawnedRooms.Count - 2].RoomType == spawnedRooms[spawnedRooms.Count - 3].RoomType)
        {
            return spawnedRooms[spawnedRooms.Count - 1].RoomType;
        }

        return RoomType.notRelevant;
    }
}
