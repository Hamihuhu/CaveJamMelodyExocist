using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRoolState = true;
        SetChildState(_factory.Idle());
    }

    private float dashCounter;
    private Vector3 direction;

    public override void EnterState()
    {
        _machine.CanMove = false;
        _machine.CanRotation = false;
        _machine.animator.SetBool(_machine.IDDash, true);
        dashCounter = 0.2f;
        Debug.Log(dashCounter);
        direction = _machine.model.forward.normalized ;
        _machine.JumpVelocity = 0;
    }
    protected override void UpdateState()
    {
        dashCounter = dashCounter > 0 ? dashCounter - Time.deltaTime : 0;
        if (dashCounter <= 0)
        {
       
            CheckSwitchState();
            return;
        }
        
       
        _machine.characterController.Move(direction * Time.deltaTime*20);
    }
    protected override void ExitState()
    {
        _machine.ReleaseAction();
        _machine.JumpVelocity = _machine.IsGrounded ? -9.81f : -30f;
 
        _machine.CanMove = true;
        _machine.CanRotation = true;
        _machine.InputMovement = Vector3.zero;
        _machine.AppliedMovement = Vector3.zero;
        _machine.animator.SetBool(_machine.IDDash, false);
    }
    public override void CheckSwitchState()
    {

        SwitchState(_factory.Grounded());
     
   
    }


}