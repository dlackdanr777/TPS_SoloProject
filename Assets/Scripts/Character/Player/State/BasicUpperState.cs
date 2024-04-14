
/// <summary> ��ü �⺻ ���¸� ������ Ŭ���� </summary>
public class BasicUpperState : PlayerUpperState
{
    public BasicUpperState(Player player, PlayerStateMachine machine) : base(player, machine) { }


    public override void OnStart()
    {
    }


    public override void OnUpdate()
    {
    }


    public override void OnFixedUpdate()
    {
    }


    public override void OnExit()
    {
    }


    public override void OnStateUpdate()
    {
        _machine.ChangeToAimModeState();
        if (_player.GunController.IsReload) _machine.ChangeState(_machine.ReloadState);
    }
}
