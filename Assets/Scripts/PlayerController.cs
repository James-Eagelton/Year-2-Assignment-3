using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    public Transform cameraTransform;
    public Transform viewObject;

    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float initialSpeed;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; 
    public float ascendMultiplier = 2f; 
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    //Crouching
    private GameObject player;
    private float startingHeight;
    public float crouchHeight;
    public float crouchTransitionTime;
    private float currentHeight;
    private Vector3 velocity = Vector3.zero;
    private bool cantStand;

   


    void Start()
    {
        player = this.gameObject;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraTransform = Camera.main.transform;
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingHeight = player.transform.localScale.y;
        currentHeight = startingHeight;
        initialSpeed = MoveSpeed;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");
        RotateCamera();
        cameraTransform.position = viewObject.position;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }
        if (Input.GetButton("Crouch") && isGrounded)
        {
            Crouch();
        }
        else 
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            cantStand = Physics.Raycast(rayOrigin, Vector3.up, raycastDistance, groundLayer);
            if (!cantStand) 
            { 
               Stand();
            }
        }       
        player.transform.localScale = new Vector3(1f, currentHeight, 1f);
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void Crouch() 
    {
        currentHeight = Mathf.SmoothDamp(currentHeight, crouchHeight, ref velocity.y, crouchTransitionTime/1.25f * Time.fixedDeltaTime);
        MoveSpeed = Mathf.Lerp(MoveSpeed, initialSpeed / 2, crouchTransitionTime/1.25f * Time.fixedDeltaTime);
    }

    void Stand() 
    { 
        currentHeight = Mathf.SmoothDamp(currentHeight, startingHeight, ref velocity.y, crouchTransitionTime * Time.deltaTime);
        MoveSpeed = Mathf.Lerp(MoveSpeed, initialSpeed, crouchTransitionTime * Time.fixedDeltaTime);
    }
    
    void MovePlayer()
    {

        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;
        Vector3 velocity = rb.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.velocity = velocity;
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void RotateCamera()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } 
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    

}
