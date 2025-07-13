using System;
using UnityEngine;

public class HitHirBoxHandeler : MonoBehaviour
{
    private BaseEnemyStateMashine BESM;

    [field: SerializeField] private DamageBoxes[] DmgBoxes; 

    private void Start()
    {
        BESM = GetComponent<BaseEnemyStateMashine>();
        
        foreach (DamageBoxes b in DmgBoxes)
        {
            b.Hitted += HandleHitted;
        }
    }

    private void HandleHitted(HealthComponent component)
    {
        component.TakeDamage(BESM.Damage);
    }

    public void EnableBoxes()
    {
        foreach (DamageBoxes b in DmgBoxes)
        {
            b.gameObject.SetActive(true);
        }
    }
    public void DisableBoxes()
    {
        foreach (DamageBoxes b in DmgBoxes)
        {
            b.gameObject.SetActive(false);
        }
    }
}
