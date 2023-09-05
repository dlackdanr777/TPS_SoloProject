using UnityEngine.Animations.Rigging;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("컴포넌트")]
    private CharacterController _myController;
    [HideInInspector] public Animator MyAnimator;
    [HideInInspector] public GunController GunController;
    [HideInInspector] public Inventory Inventory;
    public Camera MainCamera;
    public PlayerCamera PlayerCamera;


    [Header("애니메이션 리깅")]
    public MultiAimConstraint SpineAim1;
    public MultiAimConstraint SpineAim2;
    public MultiAimConstraint SpineAim3;
    public MultiAimConstraint HeadAim;

    [Header("이동 관련 변수")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    public float RunSpeedMul;
    [SerializeField] private float _rotateSpeed;

    public PlayerStateMachine Machine;

    private void Awake()
    {
        ComponentInit();
        Machine = new PlayerStateMachine(this);
    }


    private void Update()
    {
        GravityEnable();
        Machine.OnUpdate();
    }

    private void FixedUpdate()
    {
        Machine.OnFixedUpdate();
    }

    private void ComponentInit()
    {
        _myController = GetComponent<CharacterController>();
        MyAnimator = GetComponent<Animator>();
        GunController = GetComponent<GunController>();
        Inventory = GetComponent<Inventory>();
        MainCamera = Camera.main;
    }

    public void Movement(float horizontalInput, float verticalInput, float speedMul = 1) //캐릭터의 걷게하는 함수
    {
        Vector3 moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;
        moveDir = transform.TransformDirection(moveDir) * _moveSpeed * speedMul;
        _myController.Move(moveDir * Time.deltaTime);

        MyAnimator.SetFloat("Vertical", verticalInput);
        MyAnimator.SetFloat("Horizontal", horizontalInput);
    }

    public void GravityEnable() //중력을 활성화시키는 함수
    {
        _myController.Move(new Vector3(0, Physics.gravity.y * 0.7f, 0) * Time.deltaTime);
    }

    public void PlayerRotate() //캐릭터를 마우스회전에 따라 회전시키는 함수
    {
        Vector3 cameraRotation = new Vector3(0, MainCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
    }

    public void SetRiggingWeight(float weight) //조준선을 추적하는 뼈대의 가중치를 설정하는 함수
    {
        if (SpineAim1.weight != weight)
        {
            float weightLerp = Mathf.Lerp(SpineAim1.weight, weight, Time.deltaTime * 10);
            SpineAim1.weight = weightLerp;
            SpineAim2.weight = weightLerp;
            SpineAim3.weight = weightLerp;
            HeadAim.weight = weightLerp;
        }
    }


}
