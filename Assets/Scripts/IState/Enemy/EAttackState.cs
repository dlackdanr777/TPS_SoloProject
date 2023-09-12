
public class EAttackState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;

    public EAttackState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }

    public void OnStart()
    {
        _enemy.Animator.SetBool("IsAttack", true);
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
        if (!_machine.AttackStateCondition())
            _machine.ChangeState(_machine.TrackingState);
    }

    public void OnExit()
    {
        _enemy.Animator.SetBool("IsAttack", false);
    }
}
