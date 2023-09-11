using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EIdleToTrackingState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;

    private float _transitionTimer;
    public EIdleToTrackingState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }


    public void OnStart()
    {
        _enemy.Animator.SetBool("IsWalking", false);
        _transitionTimer = 0;
    }

    public void OnUpdate()
    {
        _transitionTimer += Time.deltaTime;
    }

    public void OnFixedUpdate()
    {

    }

    public void OnStateUpdate()
    {
        if (_transitionTimer > 2)
            _machine.ChangeState(_machine.TrackingState);
    }

    public void OnExit()
    {

    }
}
