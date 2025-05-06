using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewProjectileData", menuName = "Guns/Projectiles")]
public class ProjectileData : ScriptableObject
{
    [field: SerializeField] public string ProjectileName { get; private set; } = "NewProjectile";
    [field: SerializeField] public float InitialSpeed { get; private set; } = 200;

    [field: SerializeField] public float normalDamage { get; private set; } = 20;
}
