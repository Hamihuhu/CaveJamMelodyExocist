using UnityEngine;
using UnityEngine.Windows;

public class PlayerStateMachine : MonoBehaviour
{
    public Animator animator;
    public int dashTime;
    public CharacterController characterController;
    [SerializeField] private Transform cameraObject;
    [field: SerializeField] public Transform model { get; private set; }
    public PlayerBaseState CurrentState { get; set; }
    [field: SerializeField] public Vector3 AppliedMovement { get; set; }
    public bool IsGrounded => characterController.isGrounded;
    public bool IsDash => input.Dash;
    public bool IsRun => IsGrounded && !IsIdle;
    public bool IsWalk { get; set; }
    public bool IsJump => input.Jump && IsGrounded;
    public bool CanMove { get; set; }
    public bool CanRotation { get; set; }
    //variable


    public float JumpVelocity { get; set; }
    public Vector3 InputMovement { get; set; }
    public int JumpHeight { get; internal set; }
    public float Gravity { get; private set; } = -30f;
    public bool IsIdle => input.Move.magnitude == 0;

    //variable
    [SerializeField] private float moveSpeedModifier;
    [SerializeField] private float smoothVelocityY;
    [SerializeField] private float smoothJumpTime;
    [SerializeField] private float jumpHeight;
    #region Animation IDs Paramater
    // FLOAT
    [HideInInspector] public int IDSpeed = Animator.StringToHash("Speed");
    // BOOL
    [HideInInspector] public int IDJump = Animator.StringToHash("Jump");
    [HideInInspector] public int IDFall = Animator.StringToHash("Fall");
    [HideInInspector] public int IDDead = Animator.StringToHash("Dead");
    // TRIGGER
    [HideInInspector] public int IDDamageFall = Animator.StringToHash("Damage_Fall");
    [HideInInspector] public int IDDamageStand = Animator.StringToHash("Damage_Stand");
    [HideInInspector] public int IDDash = Animator.StringToHash("Dash");
    [HideInInspector] public int IDHoldAttack = Animator.StringToHash("HoldAttack");
    private PlayerStateFactory _state;
    private Transform _mainCamera;
    [field: SerializeField] public PlayerInput input { private set; get; }

    public PlayerConfig PlayerConfig { get; set; }

    #endregion
    private void Awake()
    {
        GetReference();
        SetVariables();

    }
    protected virtual void Update()
    {
        HandleInput();
        CurrentState.UpdateStates();
        HandleMovement();
    }



    private void HandleMovement()
    {
        if (!CanMove || !characterController.enabled)
            return;

        characterController.Move(AppliedMovement * Time.deltaTime
                                 + new Vector3(0f, JumpVelocity, 0f) * Time.deltaTime);
    }

    private void HandleInput()
    {
        // Giá trị di chuyển
        var trans = transform;
        InputMovement = trans.right * input.Move.x + trans.forward * input.Move.y;
        InputMovement = Quaternion.AngleAxis(_mainCamera.transform.rotation.eulerAngles.y, Vector3.up) * InputMovement;
    }

    protected virtual void SetVariables()
    {
        // Set State
        PlayerConfig = new PlayerConfig();
        CurrentState = _state.Grounded();
        CurrentState.EnterState();
        CanMove = true;
        CanRotation = true;
    }
    protected virtual void GetReference()
    {
        characterController = GetComponent<CharacterController>();
        _state = new PlayerStateFactory(this);
        _mainCamera = Camera.main.transform;
    }
    public virtual void ReleaseAction() { }
}