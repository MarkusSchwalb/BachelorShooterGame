using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] private CapsuleCollider capsuleCollider;
    [field: SerializeField] public HealthComponent HealthComponent { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; } = 5;
    [field: SerializeField] public float SprintingSpeed { get; private set; } = 10;

    [field: SerializeField] public Recoil RecoilScript { get; private set; }

    public int currentSlot { get; private set; } = 0;

    public GameObject MainGun;
    public GameObject SecondaryGun;

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

    //public Transform MainCameraTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (rollRotator != null)
        xRotation = rollRotator.transform.localPosition.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        CheckComponents();
        SubscribeToEvent();
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
        }
        if (Camera == null)
        {
            Debug.LogError("No Camera assigned");
        }
    }

    private void SubscribeToEvent()
    {
        if (InputReader == null) { Debug.LogError("No InputReader"); return; }
        InputReader.AimEvent += HandleAimEvent;
        InputReader.AimDownEvent += HandleAimEvent;
        InputReader.ShootEvent += HandleShootEvent;
        InputReader.JumpEvent += HandleJumpEvent;
        InputReader.ReloadEvent += HandleReload;
        InputReader.MeleeEvent += HandleMelee;
        InputReader.GrenadeEvent += HandleGrenade;
        InputReader.CrouchEvent += HandleCrouch;
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
    /*
    private void HandleAimDownEvent()
    {
        throw new NotImplementedException();
    }*/

    private void HandleReload()
    {
        CurrentGun.ReloadMag();
    }

    

    private void HandleShootEvent()
    {
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
