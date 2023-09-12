using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EIdleState : IState
{
    private Enemy _enemy;
    private EnemyStateMachine _machine;

    public EIdleState(Enemy enemy, EnemyStateMachine machine)
    {
        _enemy = enemy;
        _machine = machine;
    }

    public void OnStart()
    {
        _enemy.Animator.SetBool("IsWalking", false);
        _enemy.Target = null;
    }

    public void OnUpdate()
    {
    }

public void OnFixedUpdate()
    {
        
    }

    public void OnStateUpdate()
    {
        if (_machine.ChangeTrackingStateCondition())
            _machine.ChangeState(_machine.IdleToTrackingState);

    }

    public void OnExit()
    {

    }

}
