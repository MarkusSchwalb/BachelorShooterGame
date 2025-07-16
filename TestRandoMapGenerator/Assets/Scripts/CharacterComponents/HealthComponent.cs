using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [field: SerializeField] bool isPlayerHealth = false;
    private float MaxHealth;
    public event Action<HealthComponent> DeathEvent;
    public event Action<float> DamageAction; //for staggering
    [field: SerializeField] public HealthData HDHealthData { get; private set; }
    [field: SerializeField] protected float currentHealth = 100;

    public bool IsDead { get; private set; } = false;

    [field: SerializeField] public CharacterBarSlider slider { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        if (isPlayerHealth)
        {
            MaxHealth = HDHealthData.MaxHealth * GameData.HealthModifier;
        }
        else
        {
            float multiplikator = Mathf.Clamp(GameData.CurrentLevel, 1, 5);
            MaxHealth = HDHealthData.MaxHealth * multiplikator;
        }

        
        currentHealth = MaxHealth;
        if (slider != null) slider.SetMaxValue(MaxHealth);
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
        if (currentHealth == MaxHealth) return;
        if (currentHealth > MaxHealth) { currentHealth = MaxHealth; return; }
        currentHealth += HDHealthData.HealthRegeneration * Time.deltaTime;
        UpdateSlider();
    }

    protected bool IsFullLife()
    {
        if (!IsDead) return false;
        if (currentHealth == MaxHealth) return true;
        else return false;
    }

    public void Regenerate(float value)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth + value, 0, MaxHealth);

        UpdateSlider() ;
    }

    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0 , MaxHealth);
        UpdateSlider();
    }

    public void SetHealthToMaxHealth()
    {
        currentHealth = MaxHealth;
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
