using UnityEngine;

/// <summary> 몬스터 대기 상태 클래스 </summary>
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


    /// <summary> 일정 시간 마다 좀비 대기 상태 소리를 실행하는 함수 </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.ZombieSounds.PlayZombieSoundClip(EnemySoundType.Idle);
        _playSoundTime = Random.Range(8f, 15f);
        _playSoundTimer = 0;
    }

}
