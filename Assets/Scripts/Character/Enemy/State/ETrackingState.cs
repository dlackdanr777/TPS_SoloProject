using UnityEngine;


/// <summary> ������ ���� ���¸� ������ Ŭ���� </summary>
public class ETrackingState : EnemyState
{
    private float _playSoundTime = 0;
    private float _playSoundTimer;


    public ETrackingState(Enemy enemy, EnemyStateMachine machine) : base(enemy, machine){}


    public override void OnStart()
    {
        _enemy.Animator.SetBool("IsWalking", true);
    }

    public override void OnUpdate()
    {

    }


    public override void OnFixedUpdate()
    {
        _enemy.OnTargetFollowedHandler?.Invoke();

        _playSoundTimer += Time.deltaTime;
        PlaySound();
    }


    public override void OnStateUpdate()
    {
        if (_machine.ChangeIdleStateCondition())
            _machine.ChangeState(_machine.IdleState);

        if (_machine.AttackStateCondition())
            _machine.ChangeState(_machine.AttackState);

    }


    public override void OnExit()
    {
        _playSoundTimer = 0;
    }


    /// <summary> �����ֱ⸶�� �Ҹ��� ����ϴ� �Լ� </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.EnemySounds.PlayZombieSoundClip(EnemySoundType.Tracking);
        _playSoundTime = Random.Range(4f, 5f);
        _playSoundTimer = 0;
    }
}
