using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarInput : MonoBehaviour, PlayerControls.IAvatarActions {

    [SerializeField]
    private Avatar avatar;

    private PlayerControls controls;

    protected void OnEnable() {

        controls = new PlayerControls();
        controls.Avatar.SetCallbacks(this);
        controls.Enable();
    }

    protected void OnDisable() {

        controls.Disable();
        controls.Dispose();
    }
    public void OnBuildBlock(InputAction.CallbackContext context) {
        if (context.performed) {
            avatar.BuildBlock();
        }
    }

    public void OnDestroyBlock(InputAction.CallbackContext context) {
        if (context.performed) {
            avatar.DestroyBlock();
        }
    }
    //IAvatarActions
    public void OnJump(InputAction.CallbackContext context) {

        if (context.performed) {
            avatar.Jump();
        }
    }

    public void OnLook(InputAction.CallbackContext context) {
        if (Cursor.lockState == CursorLockMode.Locked) {
            avatar.RotateBy(context.ReadValue<Vector2>());
        }
    }
    public void OnMove(InputAction.CallbackContext context) {

        avatar.SetIntendedMovement(context.ReadValue<Vector2>());

    }
    public void OnSelectHotKey(InputAction.CallbackContext context) {

    }

    public void OnTrapCursor(InputAction.CallbackContext context) {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnUntrapCursor(InputAction.CallbackContext context) {
        Cursor.lockState = CursorLockMode.None;
    }
}
