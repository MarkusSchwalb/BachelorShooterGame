using System;
using UnityEngine;

public class GunPickUp : Interactable
{
    public klassWeaponSpawnPossibility[] SpawnableWeapons;
    int slot = 0;
    public Transform ItemDisplay;

    private void Start()
    {
        if (SpawnableWeapons == null || SpawnableWeapons.Length == 0)
        {
            Debug.LogWarning("GunPickUpHasNoGuns");
            return;
        }
        ChooseAndSpawnWeapon();
        Instantiate(SpawnableWeapons[slot].WeaponObject, ItemDisplay);
        GunData gunInfo = SpawnableWeapons[slot].WeaponGunScript.GunInfo;
        interactableText = "Press E to pick up weapon: " + gunInfo.GunName + " Type " +
            gunInfo.GunType + " FireRate " + gunInfo.FiringRate + " Damage " + gunInfo.normalDamage;
    }

    private void ChooseAndSpawnWeapon()
    {
        if (SpawnableWeapons == null || SpawnableWeapons.Length == 0) return;
        int value = 0; 
        slot = 0;
        foreach (klassWeaponSpawnPossibility spawnPossibility in SpawnableWeapons)
        {
            if (spawnPossibility==null) continue;
            value += spawnPossibility.Commonclass;
        }

        int randomValue = UnityEngine.Random.Range(0, value);
        value = 0;
        foreach (klassWeaponSpawnPossibility spawnPossibility in SpawnableWeapons)
        {
            if (spawnPossibility == null)
            {
                slot++;
                continue;
            }
            value += spawnPossibility.Commonclass;
            
            if (value > randomValue)
            {
                return;
            }
            slot++;
        }
    }

    public override void Interact()
    {
        Player player = FindFirstObjectByType<Player>();
        if ( player == null)
        {
            return;
        }
        bool isMain = SpawnableWeapons[slot].WeaponGunScript.isMainWeapon;
        player.GetNewWeapon(isMain, SpawnableWeapons[slot].WeaponObject);


        Destroy(gameObject);
    }
}
