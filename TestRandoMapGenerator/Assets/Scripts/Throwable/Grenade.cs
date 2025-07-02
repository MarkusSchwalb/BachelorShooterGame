using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Grenade : Throwable
{
    public float splashDamage = 50;
    public float radius = 2f;
    public ParticleSystem particles;
    public LayerMask Mask;

    public override void Act()
    {
        Debug.Log("Grenade Acts Boom");
        particles?.Play();

        Explosion();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        rb.isKinematic = true;
        

        Destroy(gameObject, 2f);
    }

    private void Explosion()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 2f, Mask);

        foreach (var col in targets)
        {
            // Optional: sicherstellen, dass es der richtige Typ ist
            if (col.TryGetComponent<HealthComponent>(out HealthComponent enemy))
            {
                float dmg = CalculateDamage(enemy.gameObject.transform.position);
                enemy.TakeDamage(dmg);
            }
        }
    }

    private float CalculateDamage(Vector3 position)
    {
        float distance = (position - transform.position).magnitude;
        float t = Mathf.Clamp01(distance / radius); 
        float dmg = splashDamage * (1f - t * t);

        return dmg;
    }
}
