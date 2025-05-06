using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSpawnerHolder : MonoBehaviour
{
    public List<DeckungSpawner> DeckungSpawnerList = new List<DeckungSpawner>();
    public List<EnemySpawner> EnemySpawners = new List<EnemySpawner>();

    public void GetDeckungSpawner()
    {
        DeckungSpawnerList.Clear();

        GetAllDeckungsSpawner(transform);
    }

    private void GetAllDeckungsSpawner(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent<DeckungSpawner>(out DeckungSpawner component))
            {
                DeckungSpawnerList.Add(component);
            }
            GetAllDeckungsSpawner(child);  // Rekursiv for children
        }
    }

    public void GetEnemySpawner()
    {
        EnemySpawners.Clear();

        GetAllEnemySpawner(transform);
    }

    private void GetAllEnemySpawner(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent<EnemySpawner>(out EnemySpawner component))
            {
                EnemySpawners.Add(component);
            }
            GetAllEnemySpawner(child);  // Rekursiv for children
        }
    }


}
