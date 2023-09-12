using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine
{
    private Enemy _enemy;
    public IState CurrentState { get; private set; }
    public IState IdleState { get; private set; }
    public IState IdleToTrackingState { get; private set; }
    public IState TrackingState { get; private set; }
    public IState AttackState { get; private set; }
    public IState DeadState { get; private set; }

    private float _changeTimer;
    public EnemyStateMachine(Enemy enemy)
    {
        _enemy = enemy;
        StateInit();
    }

    public void OnUpdate()
    {
        CurrentState.OnStateUpdate();
        CurrentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        CurrentState.OnFixedUpdate();
    }
    
    private void StateInit()
    {
        IdleState = new EIdleState(_enemy, this);
        IdleToTrackingState = new EIdleToTrackingState(_enemy, this);
        TrackingState = new ETrackingState(_enemy, this);
        AttackState = new EAttackState(_enemy, this);

        CurrentState = IdleState;
    }

    public void ChangeState(IState nextState) //상태를 변환하는 함수(꼭이걸로 상태를 변화해야함)
    {
        if (CurrentState == nextState) //현재상태와 입력받은 상태가 같을때에는 함수를 종료한다.
        {
            Debug.Log("현재 이미 해당 상태입니다.");
            return;
        }

        CurrentState.OnExit(); //현재 상태의 OnExit를 호출
        nextState.OnStart(); //다음 상태의 OnStart를 호출
        CurrentState = nextState; //현재 상태를 다음 상태로 변환
    }

    public bool ChangeTrackingStateCondition()
    {
        if(_enemy.FieldOfView.GetTargetTransform() != null)
        {
            _enemy.Target = _enemy.FieldOfView.GetTargetTransform();
            return true;
        }
        else
        {
            if(_enemy.Target != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool ChangeIdleStateCondition()
    {
        if (_enemy.FieldOfView.GetTargetTransform() == null)
        {
            _changeTimer += Time.deltaTime;
            if (_changeTimer > 20)
            {
                _enemy.Target = _enemy.FieldOfView.GetTargetTransform();
                return true;
            }
        }
        else
        {
            _changeTimer = 0;
        }
            return false;
    }

    public bool AttackStateCondition()
    {
        return _enemy.CheckPlayerAtAttackRange();
    }
}
