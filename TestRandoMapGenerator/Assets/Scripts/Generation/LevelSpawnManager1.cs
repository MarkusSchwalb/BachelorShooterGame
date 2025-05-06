using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class LevelSpawnManager : MonoBehaviour
{
    public event Action RaumSpawned;


    [field: SerializeField] public Int32 SeedNmbr { get; private set; }

    [field: SerializeField] public Room StartRoom { get; private set; }
    [field: SerializeField] public RoomObject CurrentRoom { get; private set; }
    [field: SerializeField] public Room EndRoom { get; private set; }

    
    [field: SerializeField] public Room[] EasyRooms { get; private set; }
    [field: SerializeField] public Room[] MediumRooms { get; private set; }
    [field: SerializeField] public Room[] HardRooms { get; private set; }

    [field: SerializeField] public int MaxRooms { get; private set; } = 10;

    [field: SerializeField] public NavMeshSurface NMSurface { get; private set; }

    private List<Room> _rooms = new List<Room>();
    private List<RoomObject> _roomObjects = new List<RoomObject>();

    private Difficulty lastRoomDif = Difficulty.notDefined;
    private int changeCount = 0;
    public int forceChangeDirAt = 3;
    private direction lastDirection = direction.NotDefined;

    private int west = 0, east = 0, north = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private int counter = 0;
    public void GenerateLevel()
    {
        lastDirection = direction.NotDefined;
        counter = 0;
        

        //destroy all previous rooms
        ClearLevel();
        //Debug.LogError("NoEndRoom");
        if (!CheckBuildingBlocks()) return;
        CheckBuildingBlocks();

        StartMapGen();
        
        //spawn the rest of the rooms
        for (int i = 1; i <= MaxRooms; i++)
        {
            SpawnNextRoom();
            
            Debug.Log("For Loop Counter: "+i);
        }
        Debug.Log(counter + "SpawnNextRoom Method Counter");
        
        

        EndMapGen();

        BakeNavMesh();

        SpawnEnemys();
    }

    private void SpawnEnemys()
    {
        foreach (RoomObject rObject in _roomObjects)
        {
            rObject.SpawnEnemys();
        }
    }

    private void StartMapGen()
    {
        UnityEngine.Random.InitState(SeedNmbr);

        west = 0; east = 0; north = 0;
        changeCount = 0;

        //Spawn Start Room
        GameObject spawnedRoom = Instantiate(StartRoom.RoomObject, transform);
        SetCurrentRoom(spawnedRoom);
        _rooms.Add(StartRoom);
        

        //Spawn Rooms
        if (CurrentRoom.Exits.Length == 0) { Debug.LogError(CurrentRoom.name + "has no valid exits"); return; }
        //spawn Room
        SpawnRoom(Difficulty.easy);
    }

    private void EndMapGen()
    {
        
        //Spawn EndRoom
        GameObject spawnedRoom = Instantiate(EndRoom.RoomObject, CurrentRoom.Exits[CalculateNextExit()]);
        SetCurrentRoom(spawnedRoom);
        _rooms.Add(EndRoom);
        Debug.Log(_rooms.Count + "Anzahl der Räume");
    }

    private bool CheckBuildingBlocks()
    {
        if (StartRoom == null) { Debug.LogError("NoStartRoom"); return false; }
        if (EndRoom == null) { Debug.LogError("NoEndRoom"); return false; }
        if (EasyRooms.Length == 0) { Debug.LogError("NotEnoughEasyRooms"); return false; }
        if (MediumRooms.Length == 0) { Debug.LogError("NotEnoughMediumRooms"); return false; }
        if (HardRooms.Length == 0) { Debug.LogError("NotEnoughHardRooms"); return false; }
        return true;
    }

    public void BakeNavMesh()
    {
        if (NMSurface == null) 
        {
            NMSurface = FindObjectOfType<NavMeshSurface>();

            if (NMSurface == null )
            {
                Debug.LogError("NoNavMeshAgent found");
                return;
            }
            
        }
        NMSurface.BuildNavMesh();
    }

    private void SpawnNextRoom()
    {
        //UnityEngine.Random.InitState(SeedNmbr);
        counter++;
        //get difficulty

        int steps = MaxRooms / 3;
        //Debug.Log("Max rooms " + MaxRooms + " / 3 =" + "    " + steps);
        //Debug.Log("Room count: " + _rooms.Count);
        if (_rooms.Count -2 < steps) { SpawnRoom(Difficulty.easy); } //-2 WEIL vorher der startraum und ein easy room reingeladen wurde
        else if (_rooms.Count -2 >= steps && _rooms.Count -2 < steps * 2) { SpawnRoom(Difficulty.medium); }
        else if (_rooms.Count -2 >= steps * 2 ) { SpawnRoom(Difficulty.hard); }

        /*
        float currentDifficulty = CalculateDifficulty();
        Debug.Log(currentDifficulty);
        switch (lastRoomDif)
        {
            case Difficulty.hard:
                if (currentDifficulty > 1.5f)
                {
                    SpawnRoom(Difficulty.easy); 
                } else
                {
                    SpawnRoom(Difficulty.medium);
                }
            break;
            case Difficulty.medium:
                if (currentDifficulty > 1.5f)
                {
                    SpawnRoom(Difficulty.easy);
                }
                else
                {
                    SpawnRoom(Difficulty.hard);
                }
                break;
            case Difficulty.easy:
                if (currentDifficulty > 1.5f)
                {
                    SpawnRoom(Difficulty.medium);
                }
                else
                {
                    SpawnRoom(Difficulty.hard);
                }
                break;
            default:
                SpawnRoom(Difficulty.easy);
                break;
        }*/

            /*
            if (currentDifficulty >= 0 && currentDifficulty < 2) { SpawnRoom(Difficulty.hard); }
            if (currentDifficulty >= 2 && currentDifficulty < 3) { SpawnRoom(Difficulty.medium); }
            if (currentDifficulty >= 3 && currentDifficulty < 10) { SpawnRoom(Difficulty.easy); }*/
    }

    private void SpawnRoom(Difficulty difficulty)
    {
       
        int ranomValue;
        GameObject spawnedRoom;
        switch (difficulty)
        { 
            case Difficulty.hard:
                ranomValue = UnityEngine.Random.Range(0, HardRooms.Length);
                spawnedRoom = Instantiate(HardRooms[ranomValue].RoomObject, CurrentRoom.ExitList[CalculateNextExit()].transform);
                _rooms.Add(HardRooms[ranomValue]);
                lastRoomDif = Difficulty.hard;
                //Debug.Log("SpawnHardRoom");
                break;
            case Difficulty.medium:
                ranomValue = UnityEngine.Random.Range(0, MediumRooms.Length);
                spawnedRoom = Instantiate(MediumRooms[ranomValue].RoomObject, CurrentRoom.ExitList[CalculateNextExit()].transform);
                _rooms.Add(MediumRooms[ranomValue]);
                lastRoomDif = Difficulty.medium;
                //Debug.Log("SpawnMediumRoom");
                break;
            case Difficulty.easy:
                ranomValue = UnityEngine.Random.Range(0, EasyRooms.Length);
                spawnedRoom = Instantiate(EasyRooms[ranomValue].RoomObject, CurrentRoom.ExitList[CalculateNextExit()].transform);
                _rooms.Add(EasyRooms[ranomValue]);
                lastRoomDif = Difficulty.easy;
                //Debug.Log("SpawnEasyRoom");
                break;
        default:
                
                ranomValue = UnityEngine.Random.Range(0, EasyRooms.Length);
                spawnedRoom = Instantiate(EasyRooms[ranomValue].RoomObject, CurrentRoom.ExitList[CalculateNextExit()].transform);
                _rooms.Add(EasyRooms[ranomValue]);
                lastRoomDif = Difficulty.easy;
                
                Debug.Log("default");
                break;

        }
        SetCurrentRoom(spawnedRoom);
        
        //Debug.Log("Room Spawned: " + spawnedRoom.name);
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
        } else exitNr = 0;

        HandleSideCounter(CurrentRoom.ExitList[exitNr].ExitDirection);
        Debug.Log("Room Spawn Direction " + CurrentRoom.ExitList[exitNr].ExitDirection);

        //Mark Exit x as mainPath
        CurrentRoom.ExitList[exitNr].SetIsMainPath(true);
        CurrentRoom.FinalizeRoom(); //maybe another position?? think about it

        return exitNr;
        /*
        UnityEngine.Random.InitState(SeedNmbr);
        if (CurrentRoom != null) { CurrentRoom.CheckExits(); }

        if (east >= 1) { CurrentRoom.DeleteDirection(direction.east); }
        if (west >= 1) { CurrentRoom.DeleteDirection(direction.west); }
        if (north >= 1) { CurrentRoom.DeleteDirection(direction.north); }
        //random function
        Debug.Log("Debug List " + CurrentRoom.ExitList + "It contains x many objects x= " + CurrentRoom.ExitList.Count);
        int exitNr = UnityEngine.Random.Range(0, CurrentRoom.ExitList.Count);
        
        //add counter for east and west 
        

        if (CurrentRoom.ExitList.Count < 2) //if only one exit than it has to be the one
        {
            if (CurrentRoom.ExitList[exitNr].ExitDirection == lastDirection) changeCount++; //count the direction change
            CurrentRoom.ExitList[exitNr].ExitDirection = lastDirection;
            HandleSideCounter(CurrentRoom.ExitList[exitNr].ExitDirection);
            return exitNr; 
        }


        if (CurrentRoom.ExitList[exitNr].ExitDirection == lastDirection)
        {
            if (changeCount > forceChangeDirAt) // force direction change after x time of the same direction
            {
                //select another direction
                if (exitNr >= CurrentRoom.ExitList.Count - 1) { exitNr=0; }
                else { exitNr++; }

                HandleSideCounter(CurrentRoom.ExitList[exitNr].ExitDirection);
                changeCount = 0;

                return exitNr;
            }
            changeCount++;
        }

        HandleSideCounter(CurrentRoom.ExitList[exitNr].ExitDirection);
        return exitNr; */
    }

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

    private void SetCurrentRoom(GameObject spawnedRoom)
    {
        if (spawnedRoom.TryGetComponent<RoomObject>(out RoomObject roomObject))
        {
            CurrentRoom = roomObject;
            _roomObjects.Add(CurrentRoom);
        }
        else
        {
            Debug.LogError("Spawned Room is no RoomObject" + spawnedRoom.name);
            return;
        }
    }

    private float CalculateDifficulty()
    {
        float value = 0;
        foreach(Room room in _rooms)
        {
            value += room.Difficulty;
        }

        value = value / (_rooms.Count);
        return value;
    }

    public void GenerateNewSeed()
    {

        int newSeed = UnityEngine.Random.Range(0, 2000);
        SeedNmbr = newSeed;
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
            _rooms.Clear();
            _roomObjects.Clear();
        }
    }

    public void StepForStep()
    {
        if (!CheckBuildingBlocks()) return;
        if (CurrentRoom == EndRoom) { Debug.LogError("AllRoomsPlaced"); return; }
        if (transform.childCount == 0)
        {
            //Spawn startRoom
            StartMapGen();
            return;
        }
        if (_rooms.Count - 2 >= MaxRooms) //2 AnfangsRäume
        {
            EndMapGen();
            return;
        }
        SpawnNextRoom();
    }
}

