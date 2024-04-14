/// <summary> �÷��̾� ��ü ���¸� �����ϴ� �߻� Ŭ���� </summary>
public abstract class PlayerUpperState : IState
{
    protected Player _player;
    protected PlayerStateMachine _machine;


    public PlayerUpperState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }


    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnStateUpdate();
    public abstract void OnExit();
}
