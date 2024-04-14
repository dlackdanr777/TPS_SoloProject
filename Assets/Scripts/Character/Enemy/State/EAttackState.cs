using UnityEngine;


/// <summary> ������ ���� ���¸� ������ Ŭ���� </summary>
public class EAttackState : EnemyState
{
    private float _playSoundTime = 10;
    private float _playSoundTimer;

    public EAttackState(Enemy enemy, EnemyStateMachine machine) : base(enemy, machine){}


    public override void OnStart()
    {
        _enemy.Animator.SetBool("IsAttack", true);
        _enemy.EnemySounds.PlayZombieSoundClip(EnemySoundType.Attack);
    }


    public override void OnUpdate()
    {

    }


    public override void OnFixedUpdate()
    {
        _enemy.OnTargetFollowedHandler?.Invoke();
        PlaySound();
    }


    public override void OnStateUpdate()
    {
        if (!_machine.AttackStateCondition())
            _machine.ChangeState(_machine.TrackingState);
    }


    public override void OnExit()
    {
        _enemy.Animator.SetBool("IsAttack", false);
    }


    /// <summary> ���� �Ҹ� ��� �Լ� </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.EnemySounds.PlayZombieSoundClip(EnemySoundType.Idle);
        _playSoundTime = Random.Range(3f, 4f);
        _playSoundTimer = 0;
    }
}
