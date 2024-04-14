
/// <summary> �޸��� ���¸� ������ Ŭ���� </summary>
public class RunState : PlayerLowerState
{
    public RunState(Player player, PlayerStateMachine machine) : base(player, machine) { }


    public override void OnStart()
    {
        _player.Animator.SetBool("IsRun", true);
    }


    public override void OnUpdate()
    {
    }


    public override void OnFixedUpdate()
    {
        _player.OnMovedHandler?.Invoke(_machine.HorizontalInput, _machine.VerticalInput, _player.PlayerMovement.RunSpeedMul);
        _player.OnRotateHandler?.Invoke();
    }


    public override void OnExit()
    {
        _player.Animator.SetBool("IsRun", false);
    }


    public override void OnStateUpdate()
    {
        _machine.ChangeToIdleState();
        _machine.ChangeToWalkState();
        _machine.ChangeToCrouchState();
    }
}
