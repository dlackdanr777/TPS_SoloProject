using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class EIdleToTrackingState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;

    private float _rotateTime = 10;
    private float _transitionTime = 10;

    private float _transitionTimer;
    private float _rotateTimer;
    public EIdleToTrackingState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }


    public void OnStart()
    {
        _enemy.ZombieSounds.PlayZombieSoundClip(ZombieSounds.ZombieSoundType.Detection);
        _enemy.Animator.SetBool("IsWalking", false);

        _rotateTime = Random.Range(0.3f, 1f);
        _transitionTime = Random.Range(3f, 4f);
    }

    public void OnUpdate()
    {
        _rotateTimer += Time.deltaTime;
        _transitionTimer += Time.deltaTime;
    }

    public void OnFixedUpdate()
    {
        if(_rotateTimer > _rotateTime)
        {
            Vector3 targetDir = _enemy.Target.transform.position - _enemy.transform.position;
            _enemy.transform.rotation = Quaternion.Lerp(_enemy.transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * 0.5f);
        }
    }

    public void OnStateUpdate()
    {
        if (_transitionTimer > _transitionTime)
            _machine.ChangeState(_machine.TrackingState);
    }

    public void OnExit()
    {
        _transitionTimer = 0;
        _rotateTimer = 0;
    }
}
