using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemys;
    bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        canSpawn = false;
    }

    public void SpawnEnemy()
    {
        Debug.Log("SpawnEnemys");
        if (!canSpawn) return;
        if (Enemys.Length == 0) { Debug.LogWarning("EnemySpawner has no assigned Enemys to spawn"); return; }
        int randomNr = UnityEngine.Random.Range(0, Enemys.Length);

        Instantiate(Enemys[randomNr], transform);
    }
}
