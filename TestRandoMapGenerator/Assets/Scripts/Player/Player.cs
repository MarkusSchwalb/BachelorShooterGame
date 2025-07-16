using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] private CapsuleCollider capsuleCollider;
    [field: SerializeField] private GameObject StartWeapon;
    public Transform playerCenter;
    [field: SerializeField] public HealthComponent HealthComponent { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; } = 5;
    [field: SerializeField] public float SprintingSpeed { get; private set; } = 10;

    [field: SerializeField] public Recoil RecoilScript { get; private set; }

    public float ThrowableCount = 0;

    public int currentSlot { get; private set; } = 0;

    public GunHolder MainGunHand;
    public GunHolder SecondaryGunHand;

    [field: SerializeField] public Gun CurrentGun { get; private set;} 
    

    [field: SerializeField] public Transform ProjectileSpawner { get; private set; }
    public GameObject projectile;

    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] private GameObject rollRotator;

    [field: SerializeField] public float ySensitivity { get; private set; }
    [field: SerializeField] public float xSensitivity { get; private set; }

    [field: SerializeField] public GameObject AimAt { get; private set; }

    [field: SerializeField] public float JumpStrength { get; private set; } = 1;

    private float xAxisLookClamp = 80f;

    float xRotation = 0;


    private bool isGrounded;
    private float gravity = -9.81f;
    private Vector3 playerVelocity;

    [field: SerializeField] public Animator AimAnimator { get; private set; }
    public static readonly int isAimingHash = Animator.StringToHash("IsAiming");

    [field: SerializeField] public PlayerMelee PM { get; private set; }
    [Header("Throwable")]
    public GameObject Grenade;
    [field: SerializeField] private Transform grenadeLauncherTrans;

    [Header("Crouchen")]
    [field: SerializeField] private float standHeight = 2, couchHeight = 1, standCam = 1.6f, crouchCam = 0.9f;

    [field: SerializeField] private GameObject AudioObj;
    [field: SerializeField] private AudioClip SaveHer;
    [field: SerializeField] private AudioClip Hurt;
    //public Transform MainCameraTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        if (rollRotator != null)
        xRotation = rollRotator.transform.localPosition.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (GameData.CurrentLevel <= 1)
            SpeakText(SaveHer);
    }

    private void SpeakText(AudioClip clip)
    {
        if (AudioObj == null) return;
        GameObject go = Instantiate(AudioObj, transform.position,transform.rotation);
        AudioObject audioObject = go.GetComponent<AudioObject>();
        if ( audioObject != null)
        {
            audioObject.SoundClip = clip;
            audioObject.playSound();
        }
    }

    private void Awake()
    {
        
        CheckComponents();
        SubscribeToEvent();

        LoadGuns();
        SelectSecondary();
    }

    private void LoadGuns()
    {
        if (GameData.SecondaryGun != null)
        {
            GetNewWeapon(false, GameData.SecondaryGun.GunObject);
        }
        if (GameData.MainGun != null)
        {
            GetNewWeapon(true, GameData.MainGun.GunObject);
        }

    }

    public void SaveGuns()
    {
        if (SecondaryGunHand.Gun != null && SecondaryGunHand.Gun.GunInfo != null)
            GameData.SecondaryGun = SecondaryGunHand.Gun.GunInfo;

        if (MainGunHand.Gun != null && MainGunHand.Gun.GunInfo != null)
            GameData.MainGun = MainGunHand.Gun.GunInfo;
    }

    private void SelectSecondary()
    {
        CurrentGun = null;
        currentSlot = 1;
        MainGunHand.gameObject.SetActive(false);
        SecondaryGunHand.gameObject.SetActive(true);
        if (StartWeapon != null)
            GetNewWeapon(false ,StartWeapon);

        if (SecondaryGunHand.Gun != null)
        {
            CurrentGun = SecondaryGunHand.Gun;
        }
    }

    private void CheckComponents()
    {
        if (CharacterController == null)
        {
            CharacterController = GetComponent<CharacterController>();
        }
        if (CharacterController == null)
        {
            InputReader = GetComponent<InputReader>();
        }
        if (HealthComponent == null)
        {
            HealthComponent = GetComponent<HealthComponent>();
            if (HealthComponent != null)
            {
                HealthComponent.DamageAction += HandleDamage;
            }
        }
        if (Camera == null)
        {
            Debug.LogError("No Camera assigned");
        }
    }

    private void HandleDamage(float obj)
    {
        SpeakText(Hurt);
    }

    private void SubscribeToEvent()
    {
        if (HealthComponent != null) HealthComponent.DeathEvent += HandleDeath;
        if (InputReader == null) { Debug.LogError("No InputReader"); return; }
        InputReader.AimEvent += HandleAimEvent;
        InputReader.AimDownEvent += HandleAimEvent;
        InputReader.ShootEvent += HandleShootEvent;
        InputReader.JumpEvent += HandleJumpEvent;
        InputReader.ReloadEvent += HandleReload;
        InputReader.MeleeEvent += HandleMelee;
        InputReader.GrenadeEvent += HandleGrenade;
        InputReader.CrouchEvent += HandleCrouch;
        InputReader.ScrollEvent += SwitchWeapon;
    }

    private void OnDestroy()
    {
        if (HealthComponent != null) HealthComponent.DeathEvent -= HandleDeath;
        if (InputReader == null) { Debug.LogError("No InputReader"); return; }
        InputReader.AimEvent -= HandleAimEvent;
        InputReader.AimDownEvent -= HandleAimEvent;
        InputReader.ShootEvent -= HandleShootEvent;
        InputReader.JumpEvent -= HandleJumpEvent;
        InputReader.ReloadEvent -= HandleReload;
        InputReader.MeleeEvent -= HandleMelee;
        InputReader.GrenadeEvent -= HandleGrenade;
        InputReader.CrouchEvent -= HandleCrouch;
        InputReader.ScrollEvent -= SwitchWeapon;
    }
    private void OnDisable()
    {
        
    }

    private void HandleDeath(HealthComponent component)
    {
        XPManager.Instance?.SaveXP();
        SceneManager.LoadScene(2);
    }

    private void HandleCrouch()
    {
        bool isCrouchen = InputReader.IsCrouching;
        Vector3 camPos = RecoilScript.transform.localPosition;

        if (isCrouchen) { 
            CharacterController.height = couchHeight;
            camPos.y = crouchCam;
            if (capsuleCollider != null) 
                capsuleCollider.height = couchHeight;
        }
        else { 
            CharacterController.height = standHeight;
            camPos.y = standCam;
            if (capsuleCollider != null)
                capsuleCollider.height = standHeight;
        }

        RecoilScript.transform.localPosition = camPos;

        CharacterController.center = new Vector3 (0, CharacterController.height/2, 0);
    }

    private void HandleGrenade()
    {
        Debug.Log("FireInTheHole");
        if (grenadeLauncherTrans == null || Grenade == null) return;
        GameObject nGrenade = Instantiate(Grenade, grenadeLauncherTrans.position, grenadeLauncherTrans.rotation);

    }

    private void HandleMelee()
    {
        Debug.Log("Melee");
        if (PM == null) return;
        PM.Attack();
    }

    private void HandleAimEvent()
    {
        if ( AimAnimator == null)
        {
            return;
        }
        AimAnimator.SetBool(isAimingHash, InputReader.IsAiming);
    }

    private void SwitchWeapon()
    {
        Debug.Log("SwitchWeapon");
        if (MainGunHand != null && MainGunHand.gameObject != null)
            MainGunHand.gameObject.SetActive(!MainGunHand.gameObject.activeSelf);
        if (SecondaryGunHand != null && SecondaryGunHand.gameObject != null)
            SecondaryGunHand.gameObject.SetActive(!SecondaryGunHand.gameObject.activeSelf);

        if (SecondaryGunHand.gameObject.activeSelf)
        {
            CurrentGun = SecondaryGunHand.Gun;
            currentSlot = 1;
        }
        if (MainGunHand.gameObject.activeSelf)
        {
            CurrentGun = MainGunHand.Gun;
            currentSlot = 0;
        }
    }

    private void SwitchWeapon(bool toMain)
    {
        Debug.Log("SwitchWeapon");

        if (!toMain)
        {
            if (MainGunHand != null && MainGunHand.gameObject != null)
                MainGunHand.gameObject.SetActive(false);
            if (SecondaryGunHand != null && SecondaryGunHand.gameObject != null)
                SecondaryGunHand.gameObject.SetActive(true);
        }
        else
        {
            if (MainGunHand != null && MainGunHand.gameObject != null)
                MainGunHand.gameObject.SetActive(true);
            if (SecondaryGunHand != null && SecondaryGunHand.gameObject != null)
                SecondaryGunHand.gameObject.SetActive(false);
        }
        

        if (SecondaryGunHand.gameObject.activeSelf)
        {
            CurrentGun = SecondaryGunHand.Gun;
            currentSlot = 1;
        }
        if (MainGunHand.gameObject.activeSelf)
        {
            CurrentGun = MainGunHand.Gun;
            currentSlot = 0;
        }
    }

    public void GetNewWeapon(bool isMain, GameObject gunObject)
    {
        if (isMain)
        {
            MainGunHand.GetNewGun(gunObject);
            SwitchWeapon(true);
            //MainGunHand.Gun.Magazin.Get
        }
        else
        {
            SecondaryGunHand.GetNewGun(gunObject);
            SwitchWeapon(false);
        }
        SaveGuns();
    }

    

    /*
    private void HandleAimDownEvent()
    {
        throw new NotImplementedException();
    }*/

    private void HandleReload()
    {
        if (CurrentGun == null) return;
        CurrentGun.ReloadMag();
    }

    

    private void HandleShootEvent()
    {
        Debug.Log("HandleShootEvent");
        if (CurrentGun == null) return;
        CurrentGun.HandleFireInput();

        /*
        if (projectile == null) { return; }
        if (ProjectileSpawner ==null) { return; }

        Vector3 position = ProjectileSpawner.transform.position;
        Quaternion rotation = ProjectileSpawner.transform.rotation;

        Instantiate(projectile, position, rotation);*/
    }
    private void HandleJumpEvent()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(JumpStrength * -3 * gravity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        CameraRotation();
        Movement();
    }

    private void GroundCheck()
    {
        isGrounded = CharacterController.isGrounded;
    }

    private void CameraRotation()
    {
        Vector2 mouseInput = InputReader.MouseInput;

        if (rollRotator == null) { 
            //Debug.Log(mouseInput);
            //Up and down
            xRotation -= (mouseInput.y * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -xAxisLookClamp, xAxisLookClamp);
            Camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
        //rotate Rotators
        //up Down
        xRotation -= (mouseInput.y * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -xAxisLookClamp, xAxisLookClamp);
        rollRotator.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //left right
        transform.Rotate(Vector3.up * (mouseInput.x * Time.deltaTime) * xSensitivity);

        



    }

    private void Movement()
    {
        Vector2 moveInput = InputReader.MoveInput;
        float vertical = moveInput.y;
        float horizontal = moveInput.x;

        float SpeedModifier;
        if (InputReader.IsSprinting)
        {
            SpeedModifier = SprintingSpeed;
        }
        else
        {
            SpeedModifier = MovementSpeed;
        }
        /**/
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal);
            

        CharacterController.Move(moveDirection * SpeedModifier * Time.deltaTime);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        CharacterController.Move(playerVelocity * Time.deltaTime);

    }

    
}
