
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
        _enemy.OnTargetFollowedHandler();
    }

    public void OnStateUpdate()
    {
        if (_enemy.Target == null)
            _machine.ChangeState(_machine.IdleState);
    }

    public void OnExit()
    {
        _enemy.Animator.SetBool("IsWalking", false);
    }
}
