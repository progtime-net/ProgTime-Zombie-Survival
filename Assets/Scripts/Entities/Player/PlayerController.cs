using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//TODO: Uncomment everything and enable mirror stuff after adding network and guns
[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour, IDamageable
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int DieTrigger = Animator.StringToHash("DieTrigger");

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float moveSpeedLerp = 10f;
    [SerializeField] private float inertiaLerp = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Look")]
    public Transform cam;
    [SerializeField] private float mouseSensitivity = 40f;
    [SerializeField] private Vector2 mouseClampY = new(-90f, 90f);
    
    [Header("Animation")]
    [SerializeField] private NetworkAnimator playerAnimator;
    [SerializeField] private Animator camAnimator;
    [SerializeField] private float walkAnimationSpeed = 1f;
    [SerializeField] private float runAnimationSpeed = 1f;

    [Header("Stats")] 
    [SyncVar(hook = nameof(HealthChanged))]
    [SerializeField]
    private float health = 100f;
    
    [Header("Model")]
    [SerializeField] private GameObject playerModel;
    
    
    public Inventory Inventory {get; private set;}
    
    private CharacterController _controller;
    private InputSystem _controls;
    private Transform _t;
    private Vector2 _camPosition = new(0f, 0f);
    

    private Vector2 _moveVector;
    private Vector2 _lookVector;
    private float _velocity;
    private bool _isGrounded;

    private bool _isWalking;
    private bool _isRunning;
    private bool _isJumping;
    private bool _isAttacking;
    
    private float moveSpeed = 0f;
    private Vector3 direction = Vector3.zero;

    [SyncVar(hook = nameof(AliveStateChanged))] 
    private bool isAlive = true;
    public bool IsAlive => isAlive;

    public static PlayerController LocalPlayer { get; private set; }
    
    void Start()
    {
        GameManager.Instance.PlayerConnected(this);
        _controller = GetComponent<CharacterController>();
        _t = transform;
                
        cam.gameObject.SetActive(isLocalPlayer);
        playerModel.SetActive(!isLocalPlayer);
        
        if (!isLocalPlayer) return;
        
        
        LocalPLayer = this;
        _controls = new InputSystem();
        Inventory = new Inventory();
        

        _controls.Player.Move.performed += ctx => _moveVector = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _moveVector = Vector2.zero;

        _controls.Player.Look.performed += ctx => _lookVector = ctx.ReadValue<Vector2>();
        _controls.Player.Look.canceled += _ => _lookVector = Vector2.zero;

        _controls.Player.Jump.performed += _ => _isJumping = true;
        _controls.Player.Jump.canceled += _ => _isJumping = false;

        _controls.Player.Attack.performed += _ => _isAttacking = true;
        _controls.Player.Attack.canceled += _ => _isAttacking = false;

        _controls.Player.Sprint.performed += _ => _isRunning = true;
        _controls.Player.Sprint.canceled += _ => _isRunning = false;

        _controls.Player.Interact.performed += _ => Interact();
        _controls.Player.SelectWeapon.performed += SelectWeapon;
        _controls.Player.DropItem.performed += _ => DropItem();
        _controls.Player.ScrollWeapon.performed += ScrollWeapon;
        
        _controls.Enable();
    }

    private void Interact() { }

    private void SelectWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.control is KeyControl key)
        {
            Inventory.SetCurrentWeapon(key.keyCode - Key.Digit1);
        }
    }

    private void ScrollWeapon(InputAction.CallbackContext ctx)
    {
        Vector2 scroll = ctx.ReadValue<Vector2>();
        Inventory.SwitchWeapon((int)Mathf.Sign(scroll.y));
    }

    private void DropItem()
    {
        
    }
    
    private void OnEnable() => _controls?.Enable();
    private void OnDisable() => _controls?.Disable();

    private void OnDestroy()
    {
        GameManager.Instance.PlayerDisconnected(this);
    }

    void Update()
    {
        if (!isLocalPlayer || !isAlive) return;
        
        HandleMovement();
        HandleJump();
        HandleAnimation();

        // recoilOffset = Vector2.Lerp(recoilOffset, Vector2.zero, Time.deltaTime * recoilRecoverySpeed);
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer || !isAlive) return;

        HandleLook();
        HandleCamAnimation();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer || !isAlive) return;

        HandleAttack();
    }

    private void HandleJump()
    {
        _velocity += gravity * Time.deltaTime;
        _controller.Move(Vector3.up * (_velocity * Time.deltaTime));

        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity < 0) _velocity = -2f;

        if (_isJumping && _isGrounded && _velocity <= 0f)
        {
            _velocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerAnimator.SetTrigger(JumpTrigger);
            // CmdSetTrigger(JumpTrigger);
            _isJumping = false;
        }
    }

    private void HandleMovement()
    {
        moveSpeed = Mathf.Lerp(moveSpeed, (_isRunning ? runSpeed : walkSpeed), Time.deltaTime * moveSpeedLerp);

        Vector3 moveVectorTransformed = new Vector3(_moveVector.x, 0f, _moveVector.y);
        moveVectorTransformed.Normalize();
        moveVectorTransformed = _t.TransformDirection(moveVectorTransformed);

        _isWalking = _moveVector != Vector2.zero;
        direction = Vector3.Lerp(direction, moveVectorTransformed, Time.deltaTime * inertiaLerp);

        _controller.Move(direction * (moveSpeed * Time.deltaTime));
        
    }

    private void HandleLook()
    {
        _camPosition += _lookVector * (mouseSensitivity * Time.deltaTime) /*+ recoilOffset*/;
        _camPosition.y = Mathf.Clamp(_camPosition.y, mouseClampY.x, mouseClampY.y);
        _t.rotation = Quaternion.Euler(0f, _camPosition.x, 0f);
        cam.localRotation = Quaternion.Euler(-_camPosition.y, 0f, 0f);
    }

    private void HandleCamAnimation()
    {
        camAnimator.SetBool(IsWalking, _isWalking);
        camAnimator.speed = _isRunning ? runAnimationSpeed : walkAnimationSpeed;
    }

    private void HandleAnimation()
    {
        playerAnimator.animator.SetBool(IsWalking, _isWalking);
        playerAnimator.animator.SetBool(IsRunning, _isRunning);
    }

    private void HandleAttack()
    {
        // if (_isShooting)
        //     gun.Shoot();
    }

    public void AddRecoil(float upAmount, float sideAmount)
    {
        // recoilOffset += new Vector2(
        //     Random.Range(-sideAmount, sideAmount),
        //     -upAmount
        // );
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive || !isLocalPlayer) return;
        
        health -= damage;
        if (health <= 0)
        {
            isAlive = false;
            playerAnimator.SetTrigger(DieTrigger);
            // System.Diagnostics.Process.Start("reboot");
        }
        Debug.Log(health);
    }

    private void HealthChanged(float prev, float now)
    {
       if (!isLocalPlayer) return;
       // UIManager.Instance.UpdateHealthText($"{(int)health} HP");
    }

    private void AliveStateChanged(bool prev, bool now)
    {
        
    }
    
    // [Command]
    // void CmdSetTrigger(int trigger)
    // {
    //     RpcSetTrigger(trigger);
    // }
    //
    // [ClientRpc]
    // void RpcSetTrigger(int trigger)
    // {
    //     playerAnimator.SetTrigger(trigger);
    // }
}
