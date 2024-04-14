

/// <summary>������ ���¸� ������ ���� Ŭ���� </summary>
public class ReloadState : PlayerUpperState
{

    public ReloadState(Player player, PlayerStateMachine machine) : base(player, machine) { }


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
        if (!_player.GunController.IsReload)
            _machine.ChangeState(_machine.BasicUpperState);
    }
}
