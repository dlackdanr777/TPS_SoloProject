
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

    public void InputKey() //키를 입력받는 함수
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        CrouchKeyPressed = Input.GetKeyDown(KeyCode.LeftControl);
        RunEnable = Input.GetKey(KeyCode.LeftShift) && VerticalInput > 0.1f;
        AimModeEnable = Input.GetMouseButton(1);
    }
    public void ChangeState(ILowerState nextState) //상태를 변환하는 함수(꼭이걸로 상태를 변화해야함)
    {
        if (LowerCurrentState == nextState) //현재상태와 입력받은 상태가 같을때에는 함수를 종료한다.
        {
            Debug.Log("현재 이미 해당 상태입니다.");
            return;
        }

        LowerCurrentState.OnExit(); //현재 상태의 OnExit를 호출
        nextState.OnStart(); //다음 상태의 OnStart를 호출
        LowerCurrentState = nextState; //현재 상태를 다음 상태로 변환
    }

    public void ChangeState(IUpperState nextState) //상태를 변환하는 함수(꼭이걸로 상태를 변화해야함)
    {
        if (LowerCurrentState == nextState) //현재상태와 입력받은 상태가 같을때에는 함수를 종료한다.
        {
            Debug.Log("현재 이미 해당 상태입니다.");
            return;
        }

        UpperCurrentState.OnExit(); //현재 상태의 OnExit를 호출
        nextState.OnStart(); //다음 상태의 OnStart를 호출
        UpperCurrentState = nextState; //현재 상태를 다음 상태로 변환
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
