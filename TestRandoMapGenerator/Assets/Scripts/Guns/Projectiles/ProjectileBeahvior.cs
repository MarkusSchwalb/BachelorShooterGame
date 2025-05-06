using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBeahvior : MonoBehaviour
{
    [field: SerializeField] public ProjectileData ProjectileData { get; private set; }
    [field: SerializeField] private bool isVisualOnly = true;
    

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null ) { Debug.LogError("ProjectileHasNoRigidbody"); return; }
        if (ProjectileData == null) { Debug.LogError("PtojectileHasNoData"); return; }
        rigidBody.AddForce(transform.forward * ProjectileData.InitialSpeed, ForceMode.Impulse); // Apply instant force
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log(collision.gameObject.name + " was shot");
        if (!isVisualOnly) 
        { 
            if (collision.gameObject.TryGetComponent<HitBoxComponent> (out HitBoxComponent hitBoxComponent))
            {
                //Debug.Log("Found an HitboxComponent");
                hitBoxComponent.HandleHit(ProjectileData);
            }
        }
        Destroy(gameObject);
    }
}
