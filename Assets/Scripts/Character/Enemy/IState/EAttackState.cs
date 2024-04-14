using UnityEngine;


/// <summary> ������ ���� ���¸� ������ Ŭ���� </summary>
public class EAttackState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;
    private float _playSoundTime = 10;
    private float _playSoundTimer;


    public EAttackState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }


    public void OnStart()
    {
        _enemy.Animator.SetBool("IsAttack", true);
        _enemy.ZombieSounds.PlayZombieSoundClip(EnemySoundType.Attack);
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


    /// <summary> ���� �Ҹ� ��� �Լ� </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.ZombieSounds.PlayZombieSoundClip(EnemySoundType.Idle);
        _playSoundTime = Random.Range(3f, 4f);
        _playSoundTimer = 0;
    }
}
