using UnityEngine;

/// <summary> ���� ��� ���� Ŭ���� </summary>
public class EIdleState : EnemyState
{
    private float _playSoundTime = 10;
    private float _playSoundTimer;

    public EIdleState(Enemy enemy, EnemyStateMachine machine) : base(enemy, machine){}


    public override void OnStart()
    {
        _playSoundTime = Random.Range(8f, 15f);
        _enemy.MeshController.Play("Idle");
        _enemy.OnTargetLossHandler();
    }


    public override void OnUpdate()
    {
    }


    public override void OnFixedUpdate()
    {
        _playSoundTimer += Time.deltaTime;
        PlaySound();
    }


    public override void OnStateUpdate()
    {
        if (_machine.ChangeTrackingStateCondition())
            _machine.ChangeState(_machine.IdleToTrackingState);
    }


    public override void OnExit()
    {
        _playSoundTimer = 0;
    }


    /// <summary> ���� �ð� ���� ���� ��� ���� �Ҹ��� �����ϴ� �Լ� </summary>
    private void PlaySound()
    {
        if (_playSoundTimer <= _playSoundTime)
            return;

        _enemy.EnemySounds.PlayZombieSoundClip(EnemySoundType.Idle);
        _playSoundTime = Random.Range(8f, 15f);
        _playSoundTimer = 0;
    }

}
