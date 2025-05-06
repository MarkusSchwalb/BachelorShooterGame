using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthData", menuName = "CharacterData/HealthData")]
public class HealthData : ScriptableObject
{
    public float MaxHealth = 20;
    public float HealthRegeneration;
    public float FireRessistance = 10; //percent
}
