using System;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class DamageBoxes : MonoBehaviour
{
    public event Action<HealthComponent> Hitted;
    public HealthComponent OwnHComp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<HealthComponent>(out HealthComponent hComp))
        {
            if (hComp != OwnHComp)
            {
                Hitted?.Invoke(hComp);
            }
        }
    }
}
