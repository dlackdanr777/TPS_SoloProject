using UnityEngine;


/// <summary> �÷��̾� ������±�踦 ����, �����ϴ� Ŭ���� </summary>
public class PlayerStateMachine
{
    private Player _player;

    public float HorizontalInput;
    public float VerticalInput;
    public bool CrouchKeyPressed;
    public bool RunEnable;
    public bool AimModeEnable;

    //��ü ����
    public PlayerLowerState LowerCurrentState { get; private set; }
    public PlayerLowerState IdleState { get; private set; }
    public PlayerLowerState WalkState { get; private set; }
    public PlayerLowerState CrouchIdleState { get; private set; }
    public PlayerLowerState CrouchWalkState { get; private set; }
    public PlayerLowerState RunState { get; private set; }
    public PlayerLowerState DeadState { get; private set; }


    //��ü ����
    public PlayerUpperState UpperCurrentState { get; private set; }
    public PlayerUpperState BasicUpperState { get; private set; }
    public PlayerUpperState AimModeStartState { get; private set; }
    public PlayerUpperState AimModeLoopState { get; private set; }
    public PlayerUpperState AimModeEndState { get; private set; }
    public PlayerUpperState ReloadState { get; private set; }


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
        CrouchIdleState = new CrouchIdleState(_player, this);
        CrouchWalkState = new CrouchWalkState(_player, this);
        RunState = new RunState(_player, this);
        DeadState = new DeadState(_player, this);

        BasicUpperState = new BasicUpperState(_player, this);
        AimModeStartState = new AimModeStartState(_player, this);
        AimModeLoopState = new AimModeLoopState(_player, this);
        AimModeEndState = new AimModeEndState(_player, this);
        ReloadState = new ReloadState(_player, this);
        
        LowerCurrentState = IdleState;
        UpperCurrentState = BasicUpperState;
    }


    public void InputKey()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        CrouchKeyPressed = Input.GetKeyDown(KeyCode.LeftControl);
        RunEnable = Input.GetKey(KeyCode.LeftShift) && VerticalInput > 0.1f && UpperCurrentState != ReloadState
            && LowerCurrentState != CrouchIdleState && LowerCurrentState != CrouchWalkState;
        AimModeEnable = Input.GetMouseButton(1) && LowerCurrentState != RunState && UpperCurrentState != ReloadState && !PopupUIManager.PopupEnable;
    }


    /// <summary> ��ü ���¸� ��ȭ��Ű�� �Լ� </summary>
    public void ChangeState(PlayerLowerState nextState)
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


    /// <summary> ��ü ���¸� ��ȭ��Ű�� �Լ� </summary>
    public void ChangeState(PlayerUpperState nextState)
    {
        if (UpperCurrentState == nextState) //������¿� �Է¹��� ���°� ���������� �Լ��� �����Ѵ�.
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
        if (CrouchKeyPressed)
            ChangeState(CrouchIdleState);
    }


    public void ChangeToAimModeState()
    {
        if (AimModeEnable)
            ChangeState(AimModeStartState);
    }

}
