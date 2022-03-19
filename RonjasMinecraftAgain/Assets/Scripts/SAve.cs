using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestInterfaces;
using UnityEngine.InputSystem;

public class Avatar2 : MonoBehaviour, IMovableAvatar, IRotatableAvatar {
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform head;

    [Header("Movement")]
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float movementSpeed = 10;

    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector2 intendedMovement = Vector2.zero;
    [SerializeField] private Vector2 look = Vector2.zero;

    [Header("Camera Stuff")]
    [SerializeField]
    private Vector2 cameraSensitivity = Vector2.one;
    [SerializeField]
    private float minXAngle = -45;
    [SerializeField]
    private float maxXAngle = 45;



    // Start is called before the first frame update
    void Start() {
        characterController = GetComponent<CharacterController>();

    }

    void FixedUpdate() {
        velocity.x = intendedMovement.x * movementSpeed;
        velocity.z = intendedMovement.y * movementSpeed;

        if (!characterController.isGrounded) {
            velocity += Physics.gravity * Time.deltaTime;
        }


        characterController.Move(velocity * Time.deltaTime);

        var movement = transform.rotation * new Vector3(intendedMovement.x, 0, intendedMovement.y);
    }
    public Vector3 GetVelocity() {
        return velocity;
    }

    public void Jump() {
        if (characterController.isGrounded)
            velocity.y = jumpSpeed;
    }

    public void SetIntendedMovement(Vector2 intendedMovement) {
        this.intendedMovement = intendedMovement;
    }


    public void RotateBy(Vector2 delta) {
        look.x += delta.y * cameraSensitivity.y;
        look.y += delta.x * cameraSensitivity.x;

        look.x = Mathf.Clamp(look.x, minXAngle, maxXAngle);

        head.transform.localRotation = Quaternion.Euler(look.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, look.y, 0);
    }

    [Header("Build & Destroy configurations")]
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private int interactDistance = 7;
    public void BuildBlock() {
        if (Physics.Raycast(head.transform.position, head.forward, out var hit)) {
            if (hit.distance <= interactDistance) {

            }
        }
    }

    public Quaternion GetBodyRotation() {
        throw new System.NotImplementedException();
    }

    public Quaternion GetHeadRotation() {
        throw new System.NotImplementedException();
    }
}
