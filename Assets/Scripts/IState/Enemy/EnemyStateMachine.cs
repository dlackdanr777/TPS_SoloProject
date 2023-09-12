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

    public void ChangeState(IState nextState) //���¸� ��ȯ�ϴ� �Լ�(���̰ɷ� ���¸� ��ȭ�ؾ���)
    {
        if (CurrentState == nextState) //������¿� �Է¹��� ���°� ���������� �Լ��� �����Ѵ�.
        {
            Debug.Log("���� �̹� �ش� �����Դϴ�.");
            return;
        }

        CurrentState.OnExit(); //���� ������ OnExit�� ȣ��
        nextState.OnStart(); //���� ������ OnStart�� ȣ��
        CurrentState = nextState; //���� ���¸� ���� ���·� ��ȯ
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
