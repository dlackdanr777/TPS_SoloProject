using UnityEngine;


/// <summary> 몬스터의 추적 상태를 정의한 클래스 </summary>
public class ETrackingState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;
    private float _playSoundTime = 0;
    private float _playSoundTimer;


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

        _playSoundTimer += Time.deltaTime;
        PlaySound();
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
        _playSoundTimer = 0;
    }


    /// <summary> 일정주기마다 소리를 재생하는 함수 </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.ZombieSounds.PlayZombieSoundClip(EnemySoundType.Tracking);
        _playSoundTime = Random.Range(4f, 5f);
        _playSoundTimer = 0;
    }
}
