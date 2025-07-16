using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[System.Serializable]
public class Deckung : MonoBehaviour
{
    public Transform SpawnTransform;
    public GameObject[] PossibleDeckungen;

    private void Start()
    {
        //StickToGround();
    }

    private void StickToGround()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 2;

        if (Physics.Raycast(origin, Vector3.down, out hit, 10f))
        {
            transform.position = hit.point;
        }
    }

    public void SpawnDeckung()
    {
        CheckSpawnTransform();

        DeletePrevious();

        int randomChoice = UnityEngine.Random.Range(0, PossibleDeckungen.Length);

        float randomYRotation = UnityEngine.Random.Range(-10, 11);
        Quaternion spawnRotation = SpawnTransform.rotation * Quaternion.Euler(0, randomYRotation, 0);

        GameObject SpawnObject = Instantiate(PossibleDeckungen[randomChoice], SpawnTransform.position, spawnRotation, SpawnTransform);

    }

    private void CheckSpawnTransform()
    {
        if (SpawnTransform != null) return;
        SpawnTransform = transform;
    }

    public void DeletePrevious() //Delete all previouse covers
    {
        if (SpawnTransform == null) SpawnTransform = transform;
        if (SpawnTransform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }
}
