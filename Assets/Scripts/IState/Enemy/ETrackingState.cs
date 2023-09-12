
public class ETrackingState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;

    public ETrackingState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }

    public void OnStart()
    {
        _enemy.Animator.SetBool("IsWalking", true);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        _enemy.OnTargetFollowedHandler?.Invoke();
    }

    public void OnStateUpdate()
    {
        if (_machine.ChangeIdleStateCondition())
            _machine.ChangeState(_machine.IdleState);

        if (_machine.AttackStateCondition())
            _machine.ChangeState(_machine.AttackState);

    }

    public void OnExit()
    {
    }
}
