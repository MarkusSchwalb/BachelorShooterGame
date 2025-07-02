using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
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
    [field: SerializeField] private GameObject xRotator;
    [field: SerializeField] private GameObject yRotator;

    [field: SerializeField] public float ySensitivity { get; private set; }
    [field: SerializeField] public float xSensitivity { get; private set; }



    [field: SerializeField] public float JumpStrength { get; private set; } = 1;

    private float xAxisLookClamp = 80f;

    float xRotation = 0;


    private bool isGrounded;
    private float gravity = -9.81f;
    private Vector3 playerVelocity;

    //public Transform MainCameraTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
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
        InputReader.ShootEvent += HandleShootEvent;
        InputReader.JumpEvent += HandleJumpEvent;
        InputReader.ReloadEvent += HandleReload;
    }

    private void HandleReload()
    {
        CurrentGun.ReloadMag();
    }

    private void HandleAimEvent()
    {
        Debug.Log("Aim");
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
        //Debug.Log(mouseInput);
        //Up and down
        xRotation -= (mouseInput.y * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -xAxisLookClamp, xAxisLookClamp);
        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

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
