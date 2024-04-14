
/// <summary> 몬스터의 사망 상태를 정의한 클래스 </summary>
public class EDeadState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;

    public EDeadState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
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
