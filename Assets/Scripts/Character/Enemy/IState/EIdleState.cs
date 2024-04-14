using UnityEngine;

/// <summary> ���� ��� ���� Ŭ���� </summary>
public class EIdleState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;
    private float _playSoundTime = 10;
    private float _playSoundTimer;


    public EIdleState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }


    public void OnStart()
    {
        _playSoundTime = Random.Range(8f, 15f);
        _enemy.Animator.SetBool("IsWalking", false);
        _enemy.OnTargetLossHandler();
    }


    public void OnUpdate()
    {

    }


    public void OnFixedUpdate()
    {
        _playSoundTimer += Time.deltaTime;
        PlaySound();
    }


    public void OnStateUpdate()
    {
        if (_machine.ChangeTrackingStateCondition())
            _machine.ChangeState(_machine.IdleToTrackingState);

    }


    public void OnExit()
    {
        _playSoundTimer = 0;
    }


    /// <summary> ���� �ð� ���� ���� ��� ���� �Ҹ��� �����ϴ� �Լ� </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.ZombieSounds.PlayZombieSoundClip(EnemySoundType.Idle);
        _playSoundTime = Random.Range(8f, 15f);
        _playSoundTimer = 0;
    }

}
