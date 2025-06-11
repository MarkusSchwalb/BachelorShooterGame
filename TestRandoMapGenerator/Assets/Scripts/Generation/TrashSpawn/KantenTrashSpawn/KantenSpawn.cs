using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KantenSpawn : MonoBehaviour
{
    [field: SerializeField] private TrashOptions options;
    [field: SerializeField] private float probability = 0.5f;

    public direction direction;
    public Transform spawnTransform;

    public void Spawn()
    {

    }

}
