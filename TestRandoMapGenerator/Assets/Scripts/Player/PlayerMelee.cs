using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [field: SerializeField] private GameObject Owner;
    [field: SerializeField] private List<HealthComponent> healthComponents = new List<HealthComponent>();

    [field: SerializeField] private DamageType damageType;
    [field: SerializeField] private float damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered trigger");
        if (other.gameObject == Owner) return;
        if (other.TryGetComponent<HealthComponent>(out HealthComponent component))
        {
            healthComponents.Add(component);
            component.DeathEvent += RemoveHealthComp;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Owner) return;
        if (other.TryGetComponent<HealthComponent>(out HealthComponent component))
        {
            RemoveHealthComp(component);
        }
    }

    private void RemoveHealthComp(HealthComponent component)
    {
        component.DeathEvent -= RemoveHealthComp; //unsubscribe

        healthComponents.Remove(component);
    }

    public void Attack()
    {
        foreach (HealthComponent component in healthComponents)
        {
            component?.TakeDamage(damageAmount);
        }
    }
}
