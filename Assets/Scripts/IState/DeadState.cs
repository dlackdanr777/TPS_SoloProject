public class DeadState : ILowerState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public DeadState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }
    public void OnStart()
    {

    }

    public void OnUpdate()
    {

    }
    public void OnFixedUpdate()
    {

    }
    public void OnStateUpdate()
    {

    }

    public void OnExit()
    {

    }
}
