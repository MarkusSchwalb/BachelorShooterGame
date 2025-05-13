using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxComponent : MonoBehaviour
{
    [field: SerializeField] public HealthComponent HComp { get; private set; }
    [field: SerializeField] public float DamageMultiplier { get; private set; } = 1; //for vulnerable spots like heads or something x2 or for armor parts *0.1

    XPManager manager;

    [field: SerializeField] private float xpGain = 10;

    // Start is called before the first frame update
    void Start()
    {
        manager = XPManager.Instance;

        if (HComp == null)
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            if (parent.TryGetComponent<HealthComponent> (out HealthComponent healthComponent))
            {
                HComp = healthComponent;
                return;
            }
            Debug.LogError("Hit Box Has No Assigned HealthComponent!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.TryGetComponent<ProjectileBeahvior> (out ProjectileBeahvior pbehavior))
        {
            if (pbehavior.ProjectileData == null) return;
            if (HComp == null) return;

            float damage = pbehavior.ProjectileData.normalDamage * DamageMultiplier;
            HComp.TakeDamage(damage);
        }
        */
    }

    public void HandleHit(ProjectileData pData)
    {
        Debug.Log("HandleHit");
        if (HComp == null) return;
        float damage = pData.normalDamage * DamageMultiplier;
        HComp.TakeDamage(damage);

        manager.GainXP((int)(xpGain * DamageMultiplier));
    }
}
