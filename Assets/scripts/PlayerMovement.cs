using UnityEngine;
using System.Text;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;

    [Header("Jump Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Cam")]
    public Transform cam;
    [SerializeField] private float mouseSense = 2f;

    private CharacterController controller;
    private Transform t;
    private Vector2 moveVector;
    private Vector2 lookVector;
    private float velocity;
    public bool jumpBtnPressed;
    private bool isGrounded;

    private float camX = 0f;
    private float camY = 0f;
    private float moveSpeed;
    private PlayerControls controls;
    private Vector3 direction = Vector3.zero;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        t = transform;

        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveVector = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveVector = Vector2.zero;

        controls.Player.Look.performed += ctx => lookVector = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookVector = Vector2.zero;

        controls.Player.Jump.performed += ctx => jumpBtnPressed = true;
        controls.Player.Jump.canceled += ctx => jumpBtnPressed = false;

        controls.Enable();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void LateUpdate()
    {
        HandleLook();
    }

    // HOW IT WORKS???
    void HandleJump()
    {
        velocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * velocity * Time.deltaTime);

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity < 0) velocity = -2f;

        if (isGrounded && jumpBtnPressed && velocity <= 0f)
        {
            velocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBtnPressed = false;
        }
    }

    void HandleMovement()
    {
        moveSpeed = walkSpeed;

        Vector3 moveVectorTransformed = new Vector3(moveVector.x, 0f, moveVector.y);
        moveVectorTransformed.Normalize();
        moveVectorTransformed = t.TransformDirection(moveVectorTransformed);

        // direction = Vector3.Lerp(direction, moveVectorTransformed, Time.deltaTime);
        direction = moveVectorTransformed;

        // // Gravity
        // if (!controller.isGrounded)
        //     direction.y -= 9.81f * Time.deltaTime;
        // else
        //     direction.y = -0.5f;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    // HOW IT WORKS???
    void HandleLook()
    {
        camX += lookVector.x * mouseSense * Time.deltaTime;
        camY += lookVector.y * mouseSense * Time.deltaTime;

        Vector2 mouseClampY = new Vector2(-90, 90);
        camY = Mathf.Clamp(camY, mouseClampY.x, mouseClampY.y);

        t.rotation = Quaternion.Euler(0f, camX, 0f);
        cam.localRotation = Quaternion.Euler(-camY, 0f, 0f);
    }
}
