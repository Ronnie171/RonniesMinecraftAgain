using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TestInterfaces;
using System;
public class UIInput : MonoBehaviour, PlayerControls.IUIActions
{
    private PlayerControls controls;

    [SerializeField]
    private HotkeyBar hotkeyBar;

    protected void OnEnable() {
        controls = new PlayerControls();
        controls.UI.SetCallbacks(this);
        controls.Enable();
    }
    protected void OnDisable() {
        controls.Disable();
        controls.Dispose();
    }

    public void OnTogglePause(InputAction.CallbackContext context) {
        if (context.performed) {
            // var gameManager = FindObjectOfType<GameManager>();
            // gameManager.isPaused = !gameManager.isPaused;
        }
    }
   

    public void OnSelectHotKey1(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 0;
        }
    }

    public void OnSelectHotKey2(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 1;
        }
    }

    public void OnSelectHotKey3(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 2;
        }
    }

    public void OnSelectHotKey4(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 3;
        }
    }

    public void OnSelectHotKey5(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 4;
        }
    }

    public void OnSelectHotKey6(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 5;
        }
    }

    public void OnSelectHotKey7(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 6;
        }
    }

    public void OnSelectHotKey8(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 7;
        }
    }

    public void OnSelectHotKey9(InputAction.CallbackContext context) {
        if (context.performed) {
            hotkeyBar.currentIndex = 8;
        }
    }

    public void OnSelectHotKeyMouse(InputAction.CallbackContext context) {
        if (context.performed){
            int sign = Math.Sign(context.ReadValue<float>());
            hotkeyBar.currentIndex = hotkeyBar.currentIndex - sign;
        }
    }
}
