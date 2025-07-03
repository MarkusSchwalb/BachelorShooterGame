using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AdvancedLevelGenerator : MonoBehaviour
{
    [field: SerializeField] public Int32 SeedNmbr { get; private set; }

    [Header("Rooms")]
    [field: SerializeField] public Room StartRoom { get; private set; }
    [field: SerializeField] public Room EndRoom { get; private set; }
    [field: SerializeField] public Room StartWeaponRoom { get; private set; }


    [field: SerializeField] public List<Room> RoomList { get; private set; } = new List<Room>();
    [field: SerializeField] public Room[] InBetweenRooms { get; private set; }

    [field: SerializeField] public Room[] RewardRooms;

    [field: SerializeField] public Room[] ClimaxRooms;

    [Header("Parameter")]
    public bool RandyRandom = false;
    [Tooltip("needs to be a number above 6 if " +
        "you make an input below 6 it will be 6")]
    public int MainRoomCount;


    //for intensity curve
    private List<int> intensityCurve = new List<int>();
    private List<bool> peakRestList = new List<bool>();
    private int goalIntensity;

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

    public void GenerateLevel()
    {
        GameData.CurrentLevel = 1;
        ResetSettings();
        CheckAndInitSeed();
        GenerateIntensityCurve(); //[x]
        StartGeneratingLevel(); // [x]
        SpawnRooms(); // [x]
        SpawnRewardRooms();
    }

    private void SpawnRewardRooms()
    {
        foreach (RoomObject roomO in spawnedRoomObjects)
        {
            if (roomO.Intensity >= 5) SpawnRewardIfPossible(roomO);
        }

    }

    private void SpawnRewardIfPossible(RoomObject roomO)
    {
        //Check for available exits
        roomO.CheckExits();
        if (roomO.ExitList.Count == 0) return;

        int randomInt = UnityEngine.Random.Range(0, roomO.ExitList.Count);
        Room toSpawnRoom = RoomSelection(RewardRooms);
        GameObject spawnedRoom = Instantiate(toSpawnRoom.RoomObject, roomO.ExitList[randomInt].transform);
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

    public void GenerateIntensityCurve()
    {
        if (MainRoomCount < 6) MainRoomCount = 6;
        /*if (MainRoomCount % 2 != 0)
        {
            MainRoomCount++;
        }*/

        GeneratePeakBoolList();
        
        intensityCurve.Clear();

        
        int lastHI = 1; // High Intensity
        int lastLI = 1; // low 
        int nextI = 0;

        for (int i = 0; i < peakRestList.Count; i++)
        {
            if (peakRestList[i] == false)
            {
                lastLI++;
                nextI = Mathf.Clamp(lastLI, 0, 4);
                
                if (nextI >= lastHI)
                {
                    nextI = lastHI - 1;
                }

                lastLI = nextI;
            }
            if (peakRestList[i] == true)
            {
                float maxStep = (10 / MainRoomCount) * (UnityEngine.Random.Range(1, 4));
                nextI = lastHI + UnityEngine.Random.Range(1, Mathf.RoundToInt(maxStep));
                lastHI = nextI;
            }


            nextI = Mathf.Clamp(nextI, 0, 10);
            intensityCurve.Add(nextI);

            
        }

        //Climax sichern
        intensityCurve[intensityCurve.Count - 1] = Mathf.Clamp((intensityCurve.Max() + 2), 5, 10);

        Debug.Log("Intensity Curve: " + string.Join(", ", intensityCurve));
    }

    private void GeneratePeakBoolList()
    {
        peakRestList.Clear();
        //start with rest than the hook with peak than a rest and than calculate the rest to the one
        // before the fore last value
        //fore last is false and last is peak but its going to be a special room
        peakRestList.Add(false);    //start of the curve
        peakRestList.Add(true);     //Hook
        peakRestList.Add(false);    //Rest after hook

        for (int i = 3; i < MainRoomCount - 2; i++) //we are 3 rooms in and the last two are 
        {
            if (peakRestList[peakRestList.Count-1] == true) //if current one is a peak
            {
                if (peakRestList[peakRestList.Count-2] == true) //if forelast is peak than its false
                {
                    peakRestList.Add(false);
                    continue;
                }
                int randomInt = UnityEngine.Random.Range(1, 11);
                if (randomInt <= 3) // 30 percent chance that 2 peaks after another
                {
                    peakRestList.Add(true);
                    continue;
                }
                peakRestList.Add(false); // else its simply rest
            }
            else
            {
                peakRestList.Add(true);
            }
        }

        peakRestList.Add(false);
        peakRestList.Add(true);
    }

    private void StartGeneratingLevel()
    {
        west = 0; east = 0; north = 0;

        //Spawn Start Room
        GameObject spawnedRoom = Instantiate(StartRoom.RoomObject, transform);
        SetCurrentRoom(spawnedRoom);
        spawnedRooms.Add(StartRoom);


        if (GameData.CurrentLevel == 1)
        {
            SpawnRoom(StartWeaponRoom, true);

            int rInt = UnityEngine.Random.Range(0, InBetweenRooms.Count());
            SpawnRoom(RoomSelection(InBetweenRooms), false);
        } else SpawnRoom(RoomSelection(InBetweenRooms), false);
    }

    private void SpawnRooms()
    {
        
        for (int i = 0; i <= MainRoomCount - 1; i++) //minus one because the Climax room is separate
        {
            //finde die aktuelle intensität
            goalIntensity = intensityCurve[i];
            //prüfe die Letzte art von Level
            List<Room> possibleRooms = CheckNextPossibleRooms(goalIntensity);
            Debug.Log("Possible rooms: " + string.Join(", ", possibleRooms));
            //select one of the rooms and Spawn him
            
            SpawnRoom(RoomSelection(possibleRooms), true);
            SpawnRoom(RoomSelection(InBetweenRooms), false);
        }

        //letzte rooms
        SpawnRoom(RoomSelection(ClimaxRooms), true);
        SpawnRoom(EndRoom, true);
    }

    private void SpawnRoom(Room toSpawnRoom, bool isMainRoom)
    {
        if (CurrentRoom == null)
        {
            Debug.LogWarning("SpawnRoom has tried to spawn Rooms without having currentRoom");
            return;
        }

        if (toSpawnRoom == null)
        {
            Debug.LogWarning("SpawnRoom has tried to spawn a Room but the Room had no roomObject");
            return;
        }

        GameObject spawnedRoomObj = Instantiate(
            toSpawnRoom.RoomObject, 
            CurrentRoom.ExitList[CalculateNextExit()].transform);

        SetCurrentRoom (spawnedRoomObj);
        if (isMainRoom)
        {
            spawnedRooms.Add(toSpawnRoom);
            CurrentRoom.FinalizeRoom(goalIntensity);
            

        } else CurrentRoom.FinalizeRoom(); 
    }

    

    //spawnedRoom = Instantiate(MediumRooms[ranomValue].RoomObject, CurrentRoom.ExitList[CalculateNextExit()].transform);

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

    private Room RoomSelection(List<Room> list)
    {
        int maxValue = 0;
        foreach (Room room in list)
        {
            maxValue += room.Commoness; 
        }
        int goal = UnityEngine.Random.Range(0, maxValue);
        int counter = 0;

        foreach(Room room in list)
        {
            counter += room.Commoness;
            if (counter > goal)
            {
                return room;
            }                    
        }

        return list[0];
    }

    private Room RoomSelection(Room[] list)
    {
        int maxValue = 0;
        foreach (Room room in list)
        {
            maxValue += room.Commoness;
        }
        int goal = UnityEngine.Random.Range(0, maxValue);
        int counter = 0;

        foreach (Room room in list)
        {
            counter += room.Commoness;
            if (counter > goal)
            {
                return room;
            }
        }

        return list[0];
    }


    private List<Room> CheckNextPossibleRooms(int i)
    {
        if (i > 10) i = 10;
        List<Room> list = new List<Room>();
        RoomType excludeRoomType = CheckExcludeRoomType();
        /*
        foreach (Room room in RoomList)
        {
            if (i - 1 <= room.Difficulty && room.Difficulty <= i + 1)
            {
                if (room.RoomType == excludeRoomType) continue;
                list.Add(room);
            }
        }*/
        int breaker = 0;
        while (list.Count < 1 || breaker > 10)
        {
            foreach (Room room in RoomList)
            {
                // intensity - unten soll kleiner sein als raum schwierigkeit
                //raumschwierigkeit soll kleiner sein als obere grenze
                if ( i - breaker <= room.Difficulty && room.Difficulty <= i + breaker) // the limits go wider with each while loop
                {
                    if (room.RoomType == excludeRoomType) continue;
                    list.Add(room);
                }
            }
            breaker++;
        }
        Debug.Log("Possible roomscount: " + list.Count + " Es wurde ein Breaker von: " 
            + breaker + "Benutzt | Diese wurden gefunden " + string.Join(", ", list));

        if (list.Count <= 0) list = RoomList; //after 10 times it should have found something but just in case
        return list;
    }

    

    private RoomType CheckExcludeRoomType()
    {
        if (spawnedRooms.Count < 2) return RoomType.notRelevant;
        if (spawnedRooms[spawnedRooms.Count - 1].RoomType == spawnedRooms[spawnedRooms.Count - 2].RoomType 
            && spawnedRooms[spawnedRooms.Count - 2].RoomType == spawnedRooms[spawnedRooms.Count - 3].RoomType)
        {
            return spawnedRooms[spawnedRooms.Count - 1].RoomType;
        }

        return RoomType.notRelevant;
    }

    private int CalculateNextExit() //momentan weiß ich doch gar nicht ob platz 0 in der Liste auch wirklich auch wirklich 0 im array ist
    {
        //Mark all exits as not path
        CurrentRoom.ResetExitBoolIsMainPath();
        //First get the exits that are not blocked
        CurrentRoom.CheckExits();
        if (CurrentRoom.ExitList.Count > 1) //Make sure we don't run in circles 3 turns right and we are where we used to be
        {
            if (east >= 1 && CurrentRoom.ExitList.Count > 1) { CurrentRoom.DeleteDirection(direction.east); } // > 1 means at lest 2
            if (west >= 1 && CurrentRoom.ExitList.Count > 1) { CurrentRoom.DeleteDirection(direction.west); }
            if (north >= 1 && CurrentRoom.ExitList.Count > 1) { CurrentRoom.DeleteDirection(direction.north); }
        }

        int exitNr;
        if (CurrentRoom.ExitList.Count > 1)
        {
            exitNr = UnityEngine.Random.Range(0, CurrentRoom.ExitList.Count);
        }
        else exitNr = 0;

        HandleSideCounter(CurrentRoom.ExitList[exitNr].ExitDirection);
        Debug.Log("Room Spawn Direction " + CurrentRoom.ExitList[exitNr].ExitDirection);

        //Mark Exit x as mainPath
        CurrentRoom.ExitList[exitNr].SetIsMainPath(true);
        

        return exitNr;

    }
    /*
    private void CalculateExits()
    {
        List<direction> notAvailable = new List<direction>();
        if (east >= 1) notAvailable.Add(direction.east);
        if (west >= 1) notAvailable.Add(direction.west);
        if (north >= 1) notAvailable.Add(direction.north);
        

        CurrentRoom.CalculateExits(notAvailable)

    }*/

    private void HandleSideCounter(direction exitDirection)
    {
        switch (exitDirection)
        {
            case direction.west:
                west++;
                east--;
                north = 0;
                break;
            case direction.east:
                east++;
                west--;
                north = 0;
                break;
            case direction.north:
                north++;
                break;
            default:
                //maybe something later
                break;
        }
        Debug.Log("north Counter: " + north + " EastCounter: " + east + " West Counter: " + west);
    }
}
