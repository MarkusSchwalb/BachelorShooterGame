using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<HealthComponent> DeathEvent;
    public event Action<float> DamageAction; //for staggering
    [field: SerializeField] public HealthData HDHealthData { get; private set; }
    [field: SerializeField] protected float currentHealth = 100;

    public bool IsDead { get; private set; } = false;

    [field: SerializeField] public CharacterBarSlider slider { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = HDHealthData.MaxHealth;
        if (slider != null) slider.SetMaxValue(HDHealthData.MaxHealth);
        UpdateSlider();
    }

    private void SetSlider()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Regenerate();
    }

    protected void Regenerate()
    {
        if (IsDead) return;
        if (currentHealth == HDHealthData.MaxHealth) return;
        if (currentHealth > HDHealthData.MaxHealth) { currentHealth = HDHealthData.MaxHealth; return; }
        currentHealth += HDHealthData.HealthRegeneration * Time.deltaTime;
        UpdateSlider();
    }

    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0 , HDHealthData.MaxHealth);
        UpdateSlider();
    }

    public void SetHealthToMaxHealth()
    {
        currentHealth = HDHealthData.MaxHealth;
        UpdateSlider();
    }

    public void TakeDamage(float value)
    {
        currentHealth -= value;
        DeathCheck();
        Debug.Log(gameObject.name + " has taken" + value + "amount of damage");
        UpdateSlider();

        DamageAction?.Invoke(value);
    }

    

    public void TakeDamage(float value, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Fire:
                value = value - (value / 100 * HDHealthData.FireRessistance);
                Mathf.Clamp(value, 0, Mathf.Infinity); //No negative damage you should not heal with damage
                currentHealth -= value ;
                break;
            //und so weiter
        }
        UpdateSlider();
    }
    
    private void DeathCheck()
    {
        if (IsDead) return; //no dying twice
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            IsDead = true;
            DeathEvent?.Invoke(this); //should invoke handle death in main script of the actor
            HandleDeath(); //should be away at some time
            
        }
    }

    private void HandleDeath() //would be better to move this to the characters7actors
    {
        

        
        Destroy(gameObject, 0.1f);
    }

    protected virtual void UpdateSlider()
    {
        //Debug.Log("UpdateSlider");
        if (slider == null) { return; }
        slider.SetSliderValue(currentHealth);
    }
}

public enum DamageType
{
    Fire,
    Poison,
    Blast,
    Regular,
    Magic
}
