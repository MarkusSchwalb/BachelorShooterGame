using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthBrain : ProceduralModulLogicBase
{
    [Tooltip("The Specific Transfor that holds the Labyrinth")]
    public Transform LabyrinthTransform;

    public LabyrinthPart[] PossibleEntrance;
    private Transform[] Entrance;

    public Transform[] PossibleExit;
    private Transform[] Exit;

    public GameObject PrototypTile;
    
    public int XGrid;
    public int ZGrid;
    public float XOffset = 2;
    public float ZOffset = 2;


    private List<LabyrinthPart> LabyrinthTiles = new List<LabyrinthPart>();

    public LabyrinthPart lastSelected {  get; private set; }
    private LabyrinthPart currentSelectedTile;

    private bool MainPath = true;

    public override void DoProcedural()
    {
        MainPath = true;
        //Spawn Labyrinth
        InitializeLabyrinth();

        //Select Entrance
        SelectEntrance();

        //Spawn Start
        SpawnStart();

        //GenerateMainWeg

    }


    private void SpawnStart()
    {
        currentSelectedTile.SpawnStart();
        
    }

    private void SelectEntrance()
    {
        SelectPossibleEntrances();
    }

    private void SelectPossibleEntrances()
    {
        PossibleEntrance = GetAllTilesFromRow(0);

        int randomint = UnityEngine.Random.Range(0, PossibleEntrance.Length);

        currentSelectedTile = GetTileOnPosition(0, randomint);
    }

    private LabyrinthPart[] GetAllTilesFromRow(int v)
    {
        List<LabyrinthPart> row = new List<LabyrinthPart>();
        foreach (LabyrinthPart tile in LabyrinthTiles)
        {
            if (tile.XRow == v)
            {
                row.Add(tile);
            }
        }

        return row.ToArray();
    }

    public void InitializeLabyrinth()
    {
        DeleteChildren();

        SpawnTiles();
    }

    private void SpawnTiles()
    {
        Vector3 spawnPosition = LabyrinthTransform.position;

        for (int i = 0; i < XGrid; i++)
        {
            spawnPosition.x = gameObject.transform.position.x + i * XOffset;

            for (int j = 0; j < ZGrid; j++)
            {
                spawnPosition.z = gameObject.transform.position.z + j * ZOffset;
                
                GameObject spawnObject = Instantiate(
                    PrototypTile, spawnPosition,
                    Quaternion.identity,
                    transform); //Spawn

                if (spawnObject.TryGetComponent<LabyrinthPart>(out LabyrinthPart part))
                {
                    LabyrinthTiles.Add(part);
                    part.initializeTile(i, j, this);
                }
            }
        }

        DelegateNeighbours();
    }

    private void DelegateNeighbours()
    {
        Debug.Log("Delegate Neighbour");
        foreach (LabyrinthPart tile in LabyrinthTiles)
        {
            tile.FindNeighbours();
        }
    }

    private void DeleteChildren()
    {
        if (LabyrinthTiles == null) LabyrinthTransform = gameObject.transform;

        if (LabyrinthTransform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }

    private void ResetTiles()
    {
        foreach (LabyrinthPart tile in LabyrinthTiles)
        {
            tile.ResetTile();
        }
    }


    public LabyrinthPart GetTileOnPosition(int x, int z)
    {
        if (x < 0 || z < 0 || x > XGrid || z > ZGrid) return null;

        foreach (LabyrinthPart tile in  LabyrinthTiles) 
        {
            if (tile.XRow == x && tile.ZRow == z) return tile;
        }

        return null;
    }
   
    public void SelectTile(LabyrinthPart tile)
    {
        lastSelected = currentSelectedTile;
        currentSelectedTile = tile;
    }
}
