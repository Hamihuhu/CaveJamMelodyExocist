using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }


    private float currentBlend;

    public override void EnterState()
    {
        _machine.AppliedMovement = Vector3.zero;
        currentBlend = _machine.animator.GetFloat(_machine.IDSpeed);

    }
    protected override void UpdateState()
    {
        currentBlend = Mathf.MoveTowards(currentBlend, 0.1f, 5f * Time.deltaTime);
        _machine.animator.SetFloat(_machine.IDSpeed, currentBlend);
        CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (_machine.IsRun)
        {
            SwitchState(_factory.Run());
        }
      
    }

}
