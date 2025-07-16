using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Gun : MonoBehaviour
{
    [field: SerializeField] public GunData GunInfo { get; private set; }
    [field: SerializeField] public bool isMainWeapon = true;
    [field: SerializeField] private bool RapidFireMode = false;
    [field: SerializeField] private float coolDown;
    [field: SerializeField] private bool ProjectileMode = false;
    [field: SerializeField] private LayerMask mask;
    [field: SerializeField] private bool CanFire;
    [field: SerializeField] public ProjectileData ProjectileDT { get; private set; }
    [field: SerializeField] private GameObject Projectile;
    [field: SerializeField] private Transform ProjectileSpawner;
    [field: SerializeField] private ParticleSystem MuscleFlash;
    [field: SerializeField] private GameObject ShotSoundObject;
    [field: SerializeField] private AudioClip GunShotAudio;
    [field: SerializeField] private AudioClip GunReloadAudio;

    [field: SerializeField] public MagazinComp Magazin { get; private set; }

    public Animator gunAnimator;
    private Player player;

    

    private int shootHash;
    private Vector3 AimAt;
    private float counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //CheckGunType();
        Magazin = GetComponent<MagazinComp>();
        gunAnimator = GetComponent<Animator>();
        if (Magazin == null) Magazin = GetComponent<MagazinComp>();
        shootHash = Animator.StringToHash("Shoot");
        GameObject goPlayer = GameObject.FindWithTag("Player");
        player = FindFirstObjectByType<Player>();
        CanFire = true;
        coolDown = 60 / GunInfo.FiringRate;
    }

    private void CheckGunType()
    {

        if (GunInfo.GunType == WeaponType.Pistol) { isMainWeapon = false; }
        else { isMainWeapon= true; }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || player.Camera == null) return;
        Ray ray = new Ray(player.Camera.transform.position, player.Camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        

        //updateRotation();

        if (!CanFire) { counter -= Time.deltaTime; }
        
        if (counter < 0) {  CanFire = true; }

        if (player.InputReader.TriggerDown && RapidFireMode) { HandleFireInput(); }
    }

    private void updateRotation()
    {
        CalCulateAimAt();
        

        gameObject.transform.rotation = Quaternion.LookRotation(AimAt);
    }

    public void HandleFireInput()
    {
        Debug.Log("HandleFireInput" + CanFire);
        if (CanFire) { Fire(); }
    }

    private void Fire()
    {
        CalCulateAimAt();
        if (!Magazin.CheckBullets()) { ShootEmpty(); return; }  
        if (ProjectileMode) { FireProjectile(); }
        if (!ProjectileMode) { FireRayMode(); }
    }

    private void ShootEmpty()
    {
        if (gunAnimator != null) { gunAnimator.SetTrigger(shootHash); }
        Debug.Log("please Reload");
    }

    private void FireRayMode()
    {
        //Do damage über ray
        RayShot();

        if ( gunAnimator != null ) { /*play animation*/ }
        
        
        //spawn visual projectile
        FireProjectile();
    }

    private void RayShot()
    {
        //Vector3 AimAtVector;
        // Get the center point of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Create ray from that point
        Ray ray = new Ray(player.Camera.transform.position, player.Camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            Debug.Log("Das wurde angeschossen" + hit.collider.gameObject.layer);
            if (hit.collider.gameObject.TryGetComponent<HitBoxComponent>(out HitBoxComponent hitBoxComponent))
            {
                hitBoxComponent.HandleHit(ProjectileDT);
            }

            // Optional: do something with the hit
            // Instantiate(effectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
        
    }

    private void FireProjectile()
    {
        if (Projectile == null) { return; }
        if (ProjectileSpawner == null) { return; }

        

        Vector3 position = ProjectileSpawner.transform.position;
        Quaternion rotation = Quaternion.LookRotation(AimAt);

        GameObject projectile =  Instantiate(Projectile, position, rotation);


        CanFire = false;
        counter = coolDown;
        ShootFeel();

        Magazin.ReduceAmmo();
    }

    

    private void ShootFeel()
    {
        if (player.RecoilScript != null) { player.RecoilScript.RecoilFire(); }
        else { Debug.Log("NoRecoilScript found"); }
        
        if (MuscleFlash != null) { MuscleFlash.Play(); }
        if (GunShotAudio != null && ShotSoundObject != null)
        {
            GameObject soundObject = Instantiate(ShotSoundObject, ProjectileSpawner.position, ProjectileSpawner.rotation);
            if (soundObject.TryGetComponent<GunShot>(out GunShot shot))
            {
                shot.SoundClip = GunShotAudio;
                shot.playSound();
            }
        }
        if (gunAnimator != null) { gunAnimator.SetTrigger(shootHash); }
    }

    private void CalCulateAimAt()
    {
        if (player.Camera == null) return;
        Vector3 AimAtVector;
        // Get the center point of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Create ray from that point
        Ray ray = new Ray(player.Camera.transform.position, player.Camera.transform.forward);
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            AimAtVector = hit.point;

            // Optional: do something with the hit
            // Instantiate(effectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else
        {
            AimAtVector = (player.Camera.transform.position + (player.Camera.transform.forward * 100f)) - player.Camera.transform.position; //otherwise aim just 100 m to the front
        }


        AimAt = AimAtVector - player.Camera.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AimAt, 0.2f);
    }

    public void ReloadMag()
    {
        //sound for reload
        if (GunReloadAudio != null && ShotSoundObject != null)
        {
            GameObject soundObject = Instantiate(ShotSoundObject, ProjectileSpawner.position, ProjectileSpawner.rotation);
            if (soundObject.TryGetComponent<GunShot>(out GunShot shot))
            {
                shot.SoundClip = GunReloadAudio;
                shot.playSound();
            }
        }

        Magazin.ReloadGun();
    }
}

public enum WeaponType
{
    Pistol,
    Shotgun,
    Sniper,
    Assaultrifle,
    MP

}
