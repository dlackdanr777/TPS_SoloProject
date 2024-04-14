using UnityEngine;

/// <summary> ����AI�� �����ϴ� Ŭ����(FSM) </summary>
public class EnemyStateMachine
{
    public IState CurrentState { get; private set; }
    public IState IdleState { get; private set; }
    public IState IdleToTrackingState { get; private set; }
    public IState TrackingState { get; private set; }
    public IState AttackState { get; private set; }
    public IState DeadState { get; private set; }

    private Enemy _enemy;
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
        DeadState = new EDeadState(_enemy, this);
        CurrentState = IdleState;
    }


    /// <summary>���¸� ��ȯ�ϴ� �Լ�(���̰ɷ� ���¸� ��ȭ�ؾ���)</summary>
    public void ChangeState(IState nextState) 
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


    /// <summary> ���� ���·� ������ �� ������ ��, �ƴϸ� ������ ��ȯ�ϴ� �Լ� </summary>
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
                return true;

            return false;
        }
    }


    /// <summary> ��� ���·� ������ �� ������ ��, �ƴϸ� ������ ��ȯ�ϴ� �Լ� </summary>
    public bool ChangeIdleStateCondition()
    {
        if (_enemy.FieldOfView.GetTargetTransform() == null)
        {
            _changeTimer += Time.deltaTime;
            if (100 < _changeTimer)
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


    /// <summary> ���� ���·� ������ �� ������ ��, �ƴϸ� ������ ��ȯ�ϴ� �Լ� </summary>
    public bool AttackStateCondition()
    {
        return _enemy.Attack.GetCheckPlayerAtAttackRange();
    }
}
