using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        if (Enemys.Length == 0) { Debug.LogWarning("EnemySpawner has no assigned Enemys to spawn"); return; }
        int randomNr = UnityEngine.Random.Range(0, Enemys.Length);

        Instantiate(Enemys[randomNr], transform);
    }
}
