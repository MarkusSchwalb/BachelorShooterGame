using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Guns/GunData")]
public class GunData : ScriptableObject
{
    [field: SerializeField] public string GunName { get; private set; } = "NewGun";

    [field: SerializeField] public GameObject GunObject { get; private set; }

    [field: SerializeField] public WeaponType GunType { get; private set; } = WeaponType.Pistol;
    [Tooltip("ShotsPerMinute")]
    [field: SerializeField] public float FiringRate { get; private set; } = 1f;
    [field: SerializeField] public float normalDamage { get; private set; } = 20;

    [field: SerializeField] public int MaxAmmo { get; private set; } = 100;

    [field: SerializeField] public int MaxBulletsInMag { get; private set; } = 20;
}
