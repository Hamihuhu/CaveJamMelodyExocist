using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    public PlayerJumpState(PlayerStateMachine _currentContext, PlayerStateFactory factory)
        : base(_currentContext, factory)
    {
        _isRoolState = true;
        SetChildState(factory.Idle());
    }

    private Quaternion _targetRot;
    private float _currentAngle;


    public override void EnterState()
    {
        _machine.ReleaseAction();
        _machine.animator.SetBool(_machine.IDJump, true);
        _machine.animator.SetBool(_machine.IDFall, false);
        _machine.JumpVelocity = Mathf.Sqrt(_machine.PlayerConfig.jumpHeigh * -2 * _machine.Gravity);

        _targetRot = Quaternion.LookRotation( _machine.InputMovement, Vector3.up); ;
    }
    protected override void UpdateState()
    {
        _machine.JumpVelocity += _machine.Gravity * Time.deltaTime;
        _currentAngle = Quaternion.Angle(_machine.model.rotation, _targetRot);
        if (_currentAngle > 1f)
        {
            _machine.model.rotation = Quaternion.RotateTowards(_machine.model.rotation, _targetRot, 1000f * Time.deltaTime);
        }
        CheckSwitchState();
    }
    protected override void ExitState()
    {
        _machine.animator.SetBool(_machine.IDJump, false);
        if (_machine.IsIdle)
            _machine.animator.SetBool(_machine.IDFall, true);
    }
    public override void CheckSwitchState()
    {
        if (_machine.IsGrounded)
        {
            SwitchState(_factory.Grounded());
        }
        else if (_machine.IsDash)
        {
            SwitchState(_factory.Dash());   
        }
       
    }


}