using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
   

    protected static PlayerInput s_Instance;

    [HideInInspector]
    public bool playerControllerInputBlocked;

    protected bool m_ExternalInputBlocked;
    public PlayerInputAction PlayerInputAction { get; private set; }

    private void OnEnable()
    {
        PlayerInputAction.Player.Enable();

        PlayerInputAction.Player.Move.performed += OnMovePressed;
        PlayerInputAction.Player.Move.canceled += OnMovePressed;

        PlayerInputAction.Player.Left.performed += OnLeftClick;
        PlayerInputAction.Player.Left.canceled += OnLeftClick;

        PlayerInputAction.Player.LeftHold.performed += OnHold;

        PlayerInputAction.Player.LeftHold.canceled += OnHold;

        PlayerInputAction.Player.Right.performed += OnRightClick;
        PlayerInputAction.Player.Right.canceled += OnRightClick;

        PlayerInputAction.Player.Jump.started += OnJumpPressed;
        PlayerInputAction.Player.Jump.canceled += OnJumpPressed;

        PlayerInputAction.Player.Dash.performed += OnDash;
        PlayerInputAction.Player.Dash.canceled += OnDash;

        PlayerInputAction.Player.Escape.performed += OnEscape;
        PlayerInputAction.Player.Escape.canceled += OnEscape;

        PlayerInputAction.Player.Interact.performed += OnInteract;
        PlayerInputAction.Player.Interact.canceled += OnInteract;

        PlayerInputAction.Player.Look.performed += OnLook;
        PlayerInputAction.Player.Look.canceled += OnLook;
    }

    public bool Hold
    {
        get { return _hold && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

    private void OnHold(InputAction.CallbackContext context)
    {
        _hold = context.ReadValueAsButton();
    }

    private void OnDisable()
    {
        PlayerInputAction.Player.Move.performed -= OnMovePressed;
        PlayerInputAction.Player.Move.canceled -= OnMovePressed;

        PlayerInputAction.Player.Left.performed -= OnLeftClick;
        PlayerInputAction.Player.Left.canceled -= OnLeftClick;

        PlayerInputAction.Player.LeftHold.performed -= OnHold;
        PlayerInputAction.Player.LeftHold.canceled -= OnHold;

        PlayerInputAction.Player.Right.performed -= OnRightClick;
        PlayerInputAction.Player.Right.canceled -= OnRightClick;

        PlayerInputAction.Player.Jump.performed -= OnJumpPressed;
        PlayerInputAction.Player.Jump.canceled -= OnJumpPressed;


        PlayerInputAction.Player.Dash.performed -= OnDash;
        PlayerInputAction.Player.Dash.canceled -= OnDash;

        PlayerInputAction.Player.Escape.performed -= OnEscape;
        PlayerInputAction.Player.Escape.canceled -= OnEscape;

        PlayerInputAction.Player.Interact.performed -= OnInteract;
        PlayerInputAction.Player.Interact.canceled -= OnInteract;

        PlayerInputAction.Player.Look.performed -= OnLook;
        PlayerInputAction.Player.Look.canceled -= OnLook;



        PlayerInputAction.Player.Disable();
    }

    void Awake()
    {


        if (s_Instance == null)
            s_Instance = this;
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");

        PlayerInputAction = new PlayerInputAction();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool HaveControl()
    {
        return !m_ExternalInputBlocked;
    }

    public void ReleaseControl()
    {
        m_ExternalInputBlocked = true;
    }

    public void GainControl()
    {
        m_ExternalInputBlocked = false;
    }
    public Vector2 Move
    {
        get
        {
            if (playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return move;
        }
    }



    public bool Right
    {
        get { return _right && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

   
    public bool Tap
    {
        set { _left = value; }
        get { return _left && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }
   
    public bool Jump { get => _jump && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    public Vector2 Look { get => ( !playerControllerInputBlocked && !m_ExternalInputBlocked)?_look:Vector2.zero; }
    public bool Escape { get => escape; }

    public bool Interact { get => interact; }

    public bool Dash { get => isDash; }

    private bool _left;
    private void OnLeftClick(InputAction.CallbackContext context) => _left = context.ReadValueAsButton();

    private bool _right;
    private void OnRightClick(InputAction.CallbackContext context) => _right = context.ReadValueAsButton();

    private Vector2 move;
    private void OnMovePressed(InputAction.CallbackContext context) => move = context.ReadValue<Vector2>();

    private bool _jump;
    private void OnJumpPressed(InputAction.CallbackContext context) => _jump = context.ReadValueAsButton();


    private bool escape;
    private void OnEscape(InputAction.CallbackContext context) => escape = context.ReadValueAsButton();

    private bool interact;
    private void OnInteract(InputAction.CallbackContext context) => interact = context.ReadValueAsButton();

    private bool isDash;
    private void OnDash(InputAction.CallbackContext context)
    {
        isDash = context.ReadValueAsButton();
    }
    Vector2 _look;
    internal float dashCounter;
    private bool _hold;

    private void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

}
