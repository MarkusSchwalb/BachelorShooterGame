using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameControls.IGamePlayActions
{
    private GameControls controlls;

    public event Action ShootEvent;
    public bool TriggerDown = false;

    public event Action AimEvent;
    public event Action JumpEvent;

    public event Action ReloadEvent;

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
        if (!context.performed) { return; }
        Debug.Log("AimInput");
        AimEvent?.Invoke();
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
}
