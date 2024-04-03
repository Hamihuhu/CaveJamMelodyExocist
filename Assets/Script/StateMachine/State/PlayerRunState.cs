using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }


    private float currentBlend;


    public override void EnterState()
    {
    
        currentBlend = _machine.animator.GetFloat(_machine.IDSpeed);
    }
    protected override void UpdateState()
    {
        _machine.AppliedMovement = _machine.InputMovement.normalized * _machine.PlayerConfig.runspeed;

        currentBlend = Mathf.MoveTowards(currentBlend, 1, 5f * Time.deltaTime);
        _machine.animator.SetFloat(_machine.IDSpeed, currentBlend);

        CheckSwitchState();
    }
    protected override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
        // // Kiểm tra các trạng thái khi nhân vật đang đứng dưới đất
        if (_machine.IsIdle)
        {
            SwitchState(_factory.Idle());
        }
        else

        if (_machine.input.Dash)
        {
          SwitchState(_factory.Dash()); 
        }


    }

}