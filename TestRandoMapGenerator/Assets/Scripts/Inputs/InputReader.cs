using System;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameControls.IGamePlayActions
{
    private GameControls controlls;

    public event Action ShootEvent;
    public bool TriggerDown = false;

    public event Action AimEvent;
    public bool IsAiming = false;
    public event Action AimDownEvent;
    public event Action JumpEvent;

    public event Action ReloadEvent;

    public event Action InteractEvent;
    public event Action MeleeEvent;
    public event Action GrenadeEvent;

    public event Action CrouchEvent;
    public bool IsCrouching = false;

    public event Action ScrollEvent;


    public Vector2 MoveInput;
    public Vector2 MouseInput;

    public bool IsSprinting = false;

    private void Start()
    {
        controlls = new GameControls();
        controlls.GamePlay.SetCallbacks(this);

        controlls.GamePlay.Enable();
    }

    

    public void OnMouseMovement(InputAction.CallbackContext context)
    {
        MouseInput = context.ReadValue<Vector2>();
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        

        if (context.performed)
        {
            TriggerDown = true;
            Debug.Log("TriggerDown");

            Debug.Log("ShootInput");
            ShootEvent?.Invoke();
        }

        if (context.canceled)
        {
            TriggerDown = false;
            Debug.Log("TriggerUp");
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAiming = true;
            
            AimEvent?.Invoke();
        }

        if (context.canceled)
        {
            IsAiming = false;
            
            AimDownEvent?.Invoke();
        }
    }

    public void OnSprinting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprinting = true;
            Debug.Log("Sprint");
        }

        if (context.canceled)
        {
            IsSprinting = false;
            Debug.Log("UnSprint");
        }
    }

    public void OnJumping(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Jump!");
        JumpEvent?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Reload!");
        ReloadEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Reload!");
        InteractEvent?.Invoke();
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Reload!");
        MeleeEvent?.Invoke();
    }

    public void OnGranade(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Reload!");
        GrenadeEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsCrouching = true;
            CrouchEvent?.Invoke();
        }

        if (context.canceled)
        {
            IsCrouching = false;
            CrouchEvent?.Invoke();
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Debug.Log("Scoll!");
        ScrollEvent?.Invoke();
    }
}
