using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LabyrinthPart : MonoBehaviour
{
    public int XRow;
    public int ZRow;

    public LabyrinthBrain Brain;
    public LabyrinthPart OriginPart {  get; private set; }
    [field: SerializeField] public LabyrinthPart Top { get; private set; }
    [field: SerializeField] public LabyrinthPart Down { get; private set; }
    [field: SerializeField] public LabyrinthPart Left { get; private set; }
    [field: SerializeField] public LabyrinthPart Right { get; private set; }

    [field: SerializeField] private GameObject LabStart;
    [field: SerializeField] private GameObject topDown;
    [field: SerializeField] private GameObject leftRight;
    [field: SerializeField] private GameObject normalT;
    [field: SerializeField] private GameObject reverseT;
    [field: SerializeField] private GameObject rightT;
    [field: SerializeField] private GameObject leftT;
    [field: SerializeField] private GameObject downT;
    [field: SerializeField] private GameObject deadEndUp;
    [field: SerializeField] private GameObject deadEndDown;
    [field: SerializeField] private GameObject deadEndRight;
    [field: SerializeField] private GameObject deadEndLeft;
    [field: SerializeField] private GameObject lUp;
    [field: SerializeField] private GameObject reversLUp;
    [field: SerializeField] private GameObject lDown;
    [field: SerializeField] private GameObject reversLDown;
    [field: SerializeField] private GameObject plus;

    public List<TopDownDirection> possibleDirections = new List<TopDownDirection>();

    private bool wasVisited = false;

    public void SpawnStart()
    {
        SpawnGameObject(LabStart);
        Brain.SelectTile(Top);
    }

    private void SpawnGameObject(GameObject start)
    {
        GameObject spawnedObject = Instantiate(start, gameObject.transform);
    }

    private void SpawnGameObject(GameObject start, Vector3 position, Quaternion rotation)
    {
        GameObject spawnedObject = Instantiate(start, position, rotation, gameObject.transform);
    }

    public void NewSpawnTile(LabyrinthPart originPart)
    {
        //Check which directions are possible
        List<LabyrinthPart> availableNeighbours = new List<LabyrinthPart>();
        if (Top != null) 
        {
            if (!Top.wasVisited) availableNeighbours.Add(Top);
        }

        if (Right != null)
        {
            if (!Right.wasVisited) availableNeighbours.Add(Right);
        }
        if (Left != null)
        {
            if (!Left.wasVisited) availableNeighbours.Add(Left);
        }
        if (Down != null)
        {
            if (!Down.wasVisited) availableNeighbours.Add(Down);
        }

        //Select Tile Acording to possible Directions
        switch (availableNeighbours.Count)
        {
            case 0: Debug.Log("Sackgasse");
                break;
            case 1:
                SelectTile(availableNeighbours[0]);
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }


        //Spawn Tile

        //Select the next tile

        //Check if endtile

        TopDownDirection direction = CheckDirection(originPart);
    }

    private void SelectTile(LabyrinthPart labyrinthPart)
    {
        //What Was my Last position
        TopDownDirection where = GetLastDirection(Brain.lastSelected);
        TopDownDirection To = GetLastDirection(labyrinthPart);


    }

    private TopDownDirection GetLastDirection(LabyrinthPart tile)
    {
        
        if (Top == tile)
        {
            return TopDownDirection.Top;
        }
        if (Left == tile)
        {
            return TopDownDirection.Left;
        }
        if (Right == tile)
        {
            return TopDownDirection.Right;
        }
        if (Down == tile)
        {
            return TopDownDirection.Down;
        }
        return TopDownDirection.Down;
    }

    private void CheckPossibleDirections()
    {

    }

    private TopDownDirection CheckDirection(LabyrinthPart originPart)
    {
        if (originPart == null) Debug.LogWarning("ErrorFromCheckDirection in Labyrinth Part");

        if(originPart == Top) { return TopDownDirection.Top; }
        if (originPart == Left) {  return TopDownDirection.Left; }
        if (originPart == Right) {  return TopDownDirection.Right; }
        if (originPart == Down) { return TopDownDirection.Down; }

        return TopDownDirection.Down;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTile()
    {
        DeleteChildren();
        wasVisited = false;

    }

    public void initializeTile(int x, int z, LabyrinthBrain brain)
    {
        XRow = x; 
        ZRow = z;
        Brain = brain;
    }

    public void FindNeighbours()
    {
        Debug.Log("FindNeighbour");
        Top = Brain.GetTileOnPosition(XRow +1, ZRow);
        Down = Brain.GetTileOnPosition(XRow - 1, ZRow);
        Left = Brain.GetTileOnPosition(XRow, ZRow - 1); 
        Right = Brain.GetTileOnPosition(XRow, ZRow + 1);
    }

    private void DeleteChildren()
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            DestroyImmediate(child.gameObject);
        }

        children.Clear();
    }

    
}

public enum TopDownDirection
{
    Top,
    Down,
    Left,
    Right
}
