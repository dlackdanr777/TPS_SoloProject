
/// <summary> ������ ���¸� �����ϴ� �߻� Ŭ���� </summary>
public abstract class EnemyState : IState
{
    protected Enemy _enemy;
    protected EnemyStateMachine _machine;

    public EnemyState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnStateUpdate();
    public abstract void OnExit();
}
