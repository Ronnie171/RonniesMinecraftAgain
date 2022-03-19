using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestInterfaces;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour, IMovableAvatar, IRotatableAvatar, IBuildingAvatar {
    

    [Header("MonoBehaviour configuration")]
    [SerializeField]
    private CharacterController characterController = default;
    [SerializeField]
    private Transform head;
    //Kamera?

    [Header("Movement parameter")]
    [SerializeField] 
    private float moveSpeed = 5;
    [SerializeField]
    private float jumpSpeed = 10;

    [Header("Camera Stuff")]
    [SerializeField]
    private Vector2 cameraSensitivity = Vector2.one;
    [SerializeField]
    private float minXAngle = -45;
    [SerializeField]
    private float maxXAngle = 45;
    //Kamerempfindlichkeit?
    //Minimal- und Maximalwinkel?

    [Header("Runtime Stuff")]
    [SerializeField]
    private Vector3 intendedMovement;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector2 look = Vector2.zero;
    //Look?

   
    void Start() {
        characterController = GetComponent<CharacterController>();
        //Wohin?
    }

    // Update is called once per frame
     protected void FixedUpdate() {

        //apply rotation?

        var movement = transform.rotation * new Vector3(intendedMovement.x, 0, intendedMovement.y);

        //apply speed

        var rotatedMovement = transform.rotation * intendedMovement;
        velocity.x = rotatedMovement.x * moveSpeed;
        velocity.z = rotatedMovement.z * moveSpeed;
       


        //apply Gravity
        if (!characterController.isGrounded) {

           velocity += Physics.gravity * Time.deltaTime;
        }
        //move character
        characterController.Move(velocity * Time.deltaTime);
    
    }
//IMovable Anfang

public void SetIntendedMovement(Vector2 intendedMovement) {

        this.intendedMovement.x = intendedMovement.x;
        this.intendedMovement.z = intendedMovement.y;

    }

    public void Jump() {

        if (characterController.isGrounded) {
            velocity.y = jumpSpeed;
        }
    }

    public Vector3 GetVelocity() {

        return velocity;
    }
    //IMovable Ende

    //IRotatable Anfang

    public void RotateBy(Vector2 delta) {
        look.x += delta.y*cameraSensitivity.y;
        look.y += delta.x*cameraSensitivity.x;

        look.x = Mathf.Clamp(look.x, minXAngle, maxXAngle);

        head.transform.localRotation = Quaternion.Euler(look.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, look.y, 0);
    }

    [Header("Build and Destroy configurations")]
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private int interactDistance = 7;
    [SerializeField]
    private float extends = 0;
    private float dicklength;
    
    public Quaternion GetBodyRotation() {
        return transform.rotation;
    }
    //Wohin?
  

    public Quaternion GetHeadRotation() {
        return head.transform.localRotation;
    }
    //Wohin?

    //IRotatable Ende

    //IBuilding Anfang

    public void BuildBlock() {
        if (Physics.Raycast(head.transform.position, head.forward, out var hit)) {
            if (hit.distance <= interactDistance) {

                Vector3 position = hit.point + hit.normal/2;
                position.x = Mathf.RoundToInt(position.x);
                position.y = Mathf.RoundToInt(position.y);
                position.z = Mathf.RoundToInt(position.z);


                if (Physics.CheckBox(position,extends * Vector3.one, Quaternion.identity)) {
                    Instantiate(blockPrefab, position, Quaternion.identity);
                    Debug.Log("position");
                }
            }
        }
    }

    public void DestroyBlock() {
        if (Physics.Raycast(head.transform.position, head.forward, out var hit)) {
            if (hit.distance <= interactDistance) {
                if (hit.transform.gameObject.tag == "Block") {
                    Vector3 position = hit.point;
                    Destroy(hit.transform.gameObject);
                }
            }
        }
            }
    public void SetReach(float distance) {
        Physics.Raycast(head.transform.position, head.forward, out var hit);
        dicklength = distance;
       
    }
    //Wohin?

    public float GetReach() {
        return dicklength;

        }
    }
    //Wohin?

    //IBuilding Ende
