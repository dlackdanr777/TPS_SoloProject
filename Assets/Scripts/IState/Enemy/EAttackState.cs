
using UnityEngine;
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
        _enemy.ZombieSounds.PlayZombieSoundClip(ZombieSounds.ZombieSoundType.Attack);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        _enemy.OnTargetFollowedHandler?.Invoke();
        PlaySound();
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


    private float _playSoundTime = 10;
    private float _playSoundTimer;

    private void PlaySound()
    {
        if (_playSoundTimer > _playSoundTime)
        {
            _enemy.ZombieSounds.PlayZombieSoundClip(ZombieSounds.ZombieSoundType.Idle);
            _playSoundTime = Random.Range(3f, 4f);
            _playSoundTimer = 0;
        }
    }
}
