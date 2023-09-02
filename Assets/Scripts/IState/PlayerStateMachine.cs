
using UnityEngine;

public class PlayerStateMachine
{
    private Player _player;

    public float HorizontalInput;
    public float VerticalInput;
    public bool CrouchKeyPressed;
    public bool RunEnable;
    public bool AimModeEnable;
    public bool IsReload;

    public ILowerState LowerCurrentState { get; private set; }
    public ILowerState IdleState { get; private set; }
    public ILowerState WalkState { get; private set; }
    public ILowerState RunState { get; private set; }


    public IUpperState UpperCurrentState { get; private set; }
    public IUpperState BasicUpperState { get; private set; }
    public IUpperState AimModeStartState { get; private set; }
    public IUpperState AimModeLoopState { get; private set; }
    public IUpperState AimModeEndState { get; private set; }
    public IState ReloadState { get; private set; }

    public void OnUpdate()
    {
        InputKey();
        LowerCurrentState.OnUpdate();
        LowerCurrentState.OnStateUpdate();

        UpperCurrentState.OnUpdate();
        UpperCurrentState.OnStateUpdate();
    }

    public void OnFixedUpdate()
    {
        LowerCurrentState.OnFixedUpdate();
        UpperCurrentState.OnFixedUpdate();
    }

    public PlayerStateMachine(Player player)
    {
        _player = player;
        StateInit();
    }

    private void StateInit()
    {
        IdleState = new IdleState(_player, this);
        WalkState = new WalkState(_player, this);
        RunState = new RunState(_player, this);


        BasicUpperState = new BasicUpperState(_player, this);
        AimModeStartState = new AimModeStartState(_player, this);
        AimModeLoopState = new AimModeLoopState(_player, this);
        AimModeEndState = new AimModeEndState(_player, this);
        
        LowerCurrentState = IdleState;
        UpperCurrentState = BasicUpperState;
    }

    public void InputKey() //Ű�� �Է¹޴� �Լ�
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        CrouchKeyPressed = Input.GetKeyDown(KeyCode.LeftControl);
        RunEnable = Input.GetKey(KeyCode.LeftShift) && VerticalInput > 0.1f;
        AimModeEnable = Input.GetMouseButton(1);
    }
    public void ChangeState(ILowerState nextState) //���¸� ��ȯ�ϴ� �Լ�(���̰ɷ� ���¸� ��ȭ�ؾ���)
    {
        if (LowerCurrentState == nextState) //������¿� �Է¹��� ���°� ���������� �Լ��� �����Ѵ�.
        {
            Debug.Log("���� �̹� �ش� �����Դϴ�.");
            return;
        }

        LowerCurrentState.OnExit(); //���� ������ OnExit�� ȣ��
        nextState.OnStart(); //���� ������ OnStart�� ȣ��
        LowerCurrentState = nextState; //���� ���¸� ���� ���·� ��ȯ
    }

    public void ChangeState(IUpperState nextState) //���¸� ��ȯ�ϴ� �Լ�(���̰ɷ� ���¸� ��ȭ�ؾ���)
    {
        if (LowerCurrentState == nextState) //������¿� �Է¹��� ���°� ���������� �Լ��� �����Ѵ�.
        {
            Debug.Log("���� �̹� �ش� �����Դϴ�.");
            return;
        }

        UpperCurrentState.OnExit(); //���� ������ OnExit�� ȣ��
        nextState.OnStart(); //���� ������ OnStart�� ȣ��
        UpperCurrentState = nextState; //���� ���¸� ���� ���·� ��ȯ
    }

    public void ChangeToIdleState()
    {
        if (HorizontalInput == 0 && VerticalInput == 0)
            ChangeState(IdleState);
    }

    public void ChangeToWalkState()
    {
        if ((HorizontalInput != 0 || VerticalInput != 0) && !RunEnable)
            ChangeState(WalkState);
    }

    public void ChangeToRunState()
    {
        if (RunEnable)
            ChangeState(RunState);
    }


    public void ChangeToCrouchState()
    {

    }

    public void ChangeToAimModeState()
    {
        if (AimModeEnable)
            ChangeState(AimModeStartState);
    }

}
