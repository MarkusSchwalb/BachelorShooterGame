using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawn : MonoBehaviour
{
    [field: SerializeField] private TrashOptions options;
    [field: SerializeField] private float fallBackBrobability = 0.5f;

    [field: SerializeField] private Transform spawnTransform;

    private RoomObject roomObject;

    public void SpawnTrash()
    {
        SpawnTrash(fallBackBrobability);
    }

    public void SpawnTrash(float probability)
    {
        DeleteChildren();

        if (options == null)
        {
            Debug.Log("No trash options");
            return;
        }

        if (options.Trashes.Length < 0)
        {
            Debug.Log("TrashOptions has no options");
            return;
        }

        if (spawnTransform == null)
        {
            Debug.Log("No Transform");
            return;
        }

        // decide wether to Spawn or not
        float randomfloat = Random.value;
        if (randomfloat < probability) SpawnT();
    }

    private void SpawnT()
    {
        List<GameObject> SpawnableObjects = new List<GameObject>();
        
        if (roomObject != null) // Wenn Raum Trash optionen hat
        {
           if (roomObject.trashOptions != null)
           {
                foreach (GameObject gO in roomObject.trashOptions.Trashes)
                {
                    SpawnableObjects.Add(gO);
                }
           }
        }

        if (SpawnableObjects.Count == 0) // Wenn er keine eingibt dann das
        {
            foreach (GameObject gO in options.Trashes)
            {
                SpawnableObjects.Add(gO);
            }
        }

        int randomSelect = Random.Range(0, SpawnableObjects.Count);

        Vector3 spawnPosition = CalculateSpawnPosition();
        Quaternion spawnRotation = CalculateQuaternion();

        GameObject spawnedTrash = Instantiate(SpawnableObjects[randomSelect], spawnPosition, spawnRotation, spawnTransform);
    }

    private Quaternion CalculateQuaternion()
    {
        float randomYRotation = UnityEngine.Random.Range(-180, 181);
        Quaternion spawnRotation = spawnTransform.rotation * Quaternion.Euler(0, randomYRotation, 0);

        return spawnRotation;
    }

    private Vector3 CalculateSpawnPosition()
    {
        float randomXoffsett = Random.Range(-0.4f, 0.41f);
        float randomZoffsett = Random.Range(-0.4f, 0.41f);

        Vector3 spawnPosition = spawnTransform.position;

        spawnPosition.x += randomXoffsett;
        spawnPosition.z += randomZoffsett;

        return spawnPosition;
    }

    public void SpawnTrash(float probability, RoomObject rO)
    {
        roomObject = rO;
        SpawnTrash(probability);
    }

    public void DeleteChildren() //Delete all previouse
    {
        if (spawnTransform == null) spawnTransform = transform;
        if (spawnTransform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
