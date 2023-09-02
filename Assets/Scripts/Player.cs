using UnityEngine.Animations.Rigging;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("컴포넌트")]
    private CharacterController _myController;
    private Camera _myCamera;
    [HideInInspector] public Animator MyAnimator;
    [HideInInspector] public GunController GunController;
    public PlayerCamera PlayerCamera;

    [Header("오브젝트")]
    [SerializeField] private GameObject _centerCamera;
    [SerializeField] private Transform _crossHair;
    [SerializeField] private Transform _muzzle;

    [Header("애니메이션 리깅")]
    public MultiAimConstraint SpineAim1;
    public MultiAimConstraint SpineAim2;
    public MultiAimConstraint SpineAim3;
    public MultiAimConstraint HeadAim;

    [Header("이동 관련 변수")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;

    public bool AimModeEnable;
    public bool RunEnable;
    public bool IsReload;

    private float _horizontalInput;
    private float _verticalInput;

    public IState CurrentState { get; private set; }
    public IState IdleState { get; private set; }
    public IState WalkState { get; private set; }
    public IState RunState { get; private set; }
    public IState AimModeStartState { get; private set; }
    public IState AimModeLoopState { get; private set; }
    public IState AimModeEndState { get; private set; }

    private void Awake()
    {
        ComponentInit();
        StateInit();
    }


    private void Update()
    {
        InputKey();
        GravityEnable();
        CurrentState.OnUpdate();
        CurrentState.OnStateUpdate();
        Debug.DrawRay(_muzzle.position, _muzzle.forward * 50, Color.red);
    }

    private void FixedUpdate()
    {
        CurrentState.OnFixedUpdate();
        Debug.Log(CurrentState);
    }

    private void ComponentInit()
    {
        _myController = GetComponent<CharacterController>();
        MyAnimator = GetComponent<Animator>();
        GunController = GetComponent<GunController>();
        _myCamera = Camera.main;
    }

    private void StateInit()
    {
        IdleState = new IdleState(this);
        WalkState = new WalkState(this);
        RunState = new RunState(this);
        AimModeStartState = new AimModeStartState(this);
        AimModeLoopState = new AimModeLoopState(this);
        AimModeEndState = new AimModeEndState(this);

        CurrentState = IdleState;
    }

    public void InputKey() //키를 입력받는 함수
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        RunEnable = Input.GetKey(KeyCode.LeftShift) && _verticalInput > 0.1f && !IsReload;
        AimModeEnable = Input.GetMouseButton(1);
    }

    public void WalkMovement() //캐릭터의 걷게하는 함수
    {
        Vector3 moveDir = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
        moveDir = transform.TransformDirection(moveDir) * _moveSpeed;
        _myController.Move(moveDir * Time.deltaTime);

        MyAnimator.SetFloat("Vertical", _verticalInput);
        MyAnimator.SetFloat("Horizontal", _horizontalInput);
    }

    public void RunMovement() //캐릭터를 달리게 해주는 변수
    {
        Vector3 moveDir = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
        moveDir = transform.TransformDirection(moveDir) * _moveSpeed * _runSpeedMul;

        _myController.Move(moveDir * Time.deltaTime);

        MyAnimator.SetFloat("Vertical", _verticalInput);
        MyAnimator.SetFloat("Horizontal", _horizontalInput);
    }

    public void GravityEnable() //중력을 활성화시키는 함수
    {
        _myController.Move(new Vector3(0, Physics.gravity.y * 0.7f, 0) * Time.deltaTime);
    }

    public void PlayerRotate() //캐릭터를 마우스회전에 따라 회전시키는 함수
    {
        Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
    }

    public void CrossHairEnable() //크로스헤어를 활성화시키는 함수
    {
        if(!_crossHair.gameObject.activeSelf)
            _crossHair.gameObject.SetActive(true);

        RaycastHit hit;
        Ray ray = _myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //카메라 정중앙에 레이를 위치시킨다
        float distance = 50f;
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Vector3 hitPos = hit.point;
            _crossHair.position = hitPos;
            _crossHair.LookAt(_myCamera.transform.position);
        }
        else
        {
            _crossHair.position = _myCamera.transform.position + _myCamera.transform.forward * distance;
            _crossHair.LookAt(_myCamera.transform.position);
        }
    }

    public void CrossHairDisable() //크로스헤어를 비활성화 시키는 함수
    {
        _crossHair.gameObject.SetActive(false);
    }

    public void SetRiggingWeight(float weight) //조준선을 추적하는 뼈대의 가중치를 설정하는 함수
    {
        if(SpineAim1.weight != weight)
        {
            float weightLerp = Mathf.Lerp(SpineAim1.weight, weight, Time.deltaTime * 10);
            SpineAim1.weight = weightLerp;
            SpineAim2.weight = weightLerp;
            SpineAim3.weight = weightLerp;
            HeadAim.weight = weightLerp;
        }
    }

    public void ChangeState(IState nextState) //상태를 변환하는 함수(꼭이걸로 상태를 변화해야함)
    {
        if(CurrentState == nextState) //현재상태와 입력받은 상태가 같을때에는 함수를 종료한다.
        {
            Debug.Log("현재 이미 해당 상태입니다.");
            return;
        }

        CurrentState.OnExit(); //현재 상태의 OnExit를 호출
        nextState.OnStart(); //다음 상태의 OnStart를 호출
        CurrentState = nextState; //현재 상태를 다음 상태로 변환
    }

    public void ChangeToIdleState()
    {
        if (_horizontalInput == 0 && _verticalInput == 0)
            ChangeState(IdleState);
    }

    public void ChangeToWalkState()
    {
        if ((_horizontalInput != 0 || _verticalInput != 0) && !RunEnable)
            ChangeState(WalkState);
    }

    public void ChangeToRunState()
    {
        if (RunEnable)
            ChangeState(RunState);
    }

    public void ChangeToAimModeState()
    {
        if (AimModeEnable)
            ChangeState(AimModeStartState);
    }

}
