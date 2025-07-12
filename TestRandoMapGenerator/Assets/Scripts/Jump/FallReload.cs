using System;
using UnityEngine;

public class FallReload : MonoBehaviour
{
    [field: SerializeField] private Transform[] spawnPoint;
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggererd");
        //musst den character controller ausschalten
        if (other.gameObject.CompareTag("Player"))
        {
            if (player == null) return;
            Vector3 nextPos = GetClosestSpawnPointPos();
            Debug.Log("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = nextPos;
            player.GetComponent<CharacterController>().enabled = true;
            player.HealthComponent.TakeDamage(10);
        }
    }

    private Vector3 GetClosestSpawnPointPos()
    {
        if (spawnPoint == null || spawnPoint.Length == 0)
        {
            Debug.LogError(name + "Has no SpawnPoints");
            
            return Vector3.zero;
        }
        if (player == null) return Vector3.zero;

        Transform closest = null;
        float lastDistance = Mathf.Infinity;
        foreach(Transform point in spawnPoint)
        {
            float sqrDistance = (point.position - player.transform.position).sqrMagnitude;
            if (sqrDistance < lastDistance)
            {
                lastDistance = sqrDistance;
                closest = point;
            }
        }
        return closest.position;
    }
}
