using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemys;
    bool canSpawn = true;
    public GameObject spawnedEnemy {get; private set;}

    [field: SerializeField] private int SpawnPercentage = 80;

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

        int ranInt = UnityEngine.Random.Range(0, 101) ;
        if (ranInt > SpawnPercentage) { return; }

        //if (!canSpawn) return;
        if (Enemys.Length == 0) { Debug.LogWarning("EnemySpawner has no assigned Enemys to spawn"); return; }
        int randomNr = UnityEngine.Random.Range(0, Enemys.Length);

        spawnedEnemy = Instantiate(Enemys[randomNr], transform);
    }

    public BaseEnemyStateMashine GetBaseEnemyStateMashine()
    {
        if (spawnedEnemy == null) return null;

        return spawnedEnemy.GetComponent<BaseEnemyStateMashine>();
    }
}
