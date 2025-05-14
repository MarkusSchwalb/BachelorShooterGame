using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazinComp : MonoBehaviour
{
    private GunData GunInfo;
    private Gun gun;

    public event Action OutOfAmmo;
    

    [field: SerializeField] public int MaxAmmo { get; private set; }
    [field: SerializeField] public int CurrentAmmo { get; private set; }
    [field: SerializeField] public int MaxBulletsInMagazine { get; private set; }
    [field: SerializeField] public int CurrentBulletsInMag { get; private set; }

    private void Awake()
    {
        gun = GetComponent<Gun>();
        GunInfo = gun.GunInfo;

        GetInitialAmmo();
    }

    private void Start()
    {
        gun = GetComponent<Gun>();
        GunInfo = gun.GunInfo;

        GetInitialAmmo();

        
    }

    private void GetInitialAmmo()
    {
        //Debug.Log("GetInitialAmmo");
        if (GunInfo == null) { Debug.LogError("NoGunInfo on " + gameObject.name); return; }
        
        MaxAmmo = GunInfo.MaxAmmo;
        CurrentAmmo = MaxAmmo;
        MaxBulletsInMagazine = GunInfo.MaxBulletsInMag;
        CurrentBulletsInMag = MaxBulletsInMagazine;
    }

    public bool CheckBullets()
    {
        if (CurrentBulletsInMag > 0) { return true; }
        //Debug.LogWarning("OutofAmmo");
        OutOfAmmo?.Invoke();
        return false;
    }

    public void ReduceAmmo()
    {
        if (!CheckBullets()) {  return; }

        CurrentAmmo--;
        CurrentBulletsInMag--;
    }

    public void ReloadGun()
    {
        if (CurrentAmmo > MaxBulletsInMagazine)
        {
            CurrentBulletsInMag = MaxBulletsInMagazine;
        }
        else
        {
            CurrentBulletsInMag = CurrentAmmo;
        }
        
    }
    
    public void PickUpAmmo()
    {
        CurrentAmmo = MaxAmmo;
    }

    public void PickUpAmmo(int value)
    {
        CurrentAmmo = Mathf.Clamp(CurrentAmmo+value, 0, MaxAmmo);
    }
}
