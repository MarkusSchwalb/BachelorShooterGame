using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class JumpPlattformPlacement : ProceduralModulLogicBase
{
    

    [field: SerializeField] private float forwardBorder = 20;
    [field: SerializeField] private float sideDeviation = 5;
    [field: SerializeField] private float heightDeviation = 1;

    [field: SerializeField] private float minJumpLength = 1;
    [field: SerializeField] private float maxJumpLength = 2;

    [field: SerializeField] private GameObject gOPlattform;

    [field: SerializeField] private List<GameObject> plattfoms = new List<GameObject> ();

    [field: SerializeField] private Transform parent;

    private GameObject LastTile;

    public override void DoProcedural()
    {
        count = 0;
        plattfoms.Clear();
        LastTile = null;
        DeleteChildren();
        SpawnStartTile();
        
    }

    

    private void SpawnStartTile()
    {
        if (gOPlattform == null) return;

        Vector3 start = parent.transform.position;

        Vector3 randomNextPosition = CalculateNextGoalPoint(start, parent);

        SpawnTile(randomNextPosition);

        SpawnNextTile();
    }

    private Vector3 CalculateNextGoalPoint(Vector3 start, Transform t)
    {
        Vector3 goalPoint;
        int i = 0;
        do
        {
            i++;
            Debug.Log("do while iteration: " + i);
            goalPoint = start;

            goalPoint += t.forward * UnityEngine.Random.Range(1, 4);
            goalPoint += t.up * UnityEngine.Random.Range(-heightDeviation, heightDeviation);
            goalPoint += t.right * UnityEngine.Random.Range(-sideDeviation, sideDeviation);

            Vector3 direction = (goalPoint - start).normalized;

            float jumpDistance = UnityEngine.Random.Range(minJumpLength, maxJumpLength);

            goalPoint = start + direction * jumpDistance;
        }
        while (CheckIsInBorder(goalPoint));

        return goalPoint;
    }

    private bool CheckIsInBorder(Vector3 goalPoint)
    {
        //check height deviation
        Vector3 postion = parent.position;
        float upperBorderY = parent.position.y + heightDeviation;
        float lowerBorderY = parent.position.y - heightDeviation;
        if (goalPoint.y < upperBorderY || goalPoint.y > lowerBorderY) return false;

        //CheckSideDeviation
        //Vector3 toVector = goalPoint - postion;


        Vector3 localTargetPos = parent.transform.InverseTransformPoint(goalPoint);
        float sideDistance = localTargetPos.x;

        if (-sideDeviation < sideDistance || sideDistance < sideDeviation) return false;


        return true;
    }

    private void SpawnTile(Vector3 position)
    {
        GameObject tile = Instantiate(gOPlattform, position, parent.transform.rotation, parent);
        plattfoms.Add(tile);
        LastTile = tile;
    }

    

    

    int count ;
    private void SpawnNextTile()
    {
        //check if it is the last tile
        if (!CheckLastTile()) //if it isn't then then do this function again
        {
            //next position
            Vector3 randomNextPosition = CalculateNextGoalPoint(LastTile.transform.position, parent);
            //spawn a tile
            SpawnTile(randomNextPosition); 

            Debug.Log("nextTile");

            SpawnNextTile(); //recursive
        }
        
        
    }

    private bool CheckLastTile() //check how far to the end
    {
        count++;

        if (LastTile == null) return false;


        Vector3 potentialLanding = LastTile.transform.position + (LastTile.transform.forward * maxJumpLength);

        Vector3 localTargetPos = parent.transform.InverseTransformPoint(LastTile.transform.position);

        float forwardDistance = localTargetPos.z;
        Debug.Log(forwardDistance);
        if (forwardDistance > forwardBorder || count > 20)
        {
            if (count > 20)
            {
                Debug.Log("count");
            }
            else Debug.Log("forwardDistance");
            return true; // if its under the
        } 

        return false;
    }

    public void DeleteChildren() //Delete all previouse
    {
        if (parent == null) parent = transform;
        if (parent.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }
}
