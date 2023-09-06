using UnityEngine.Animations.Rigging;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float RunSpeedMul => _runSpeedMul; 

    [Header("컴포넌트")]
    public Camera MainCamera;
    public PlayerCamera PlayerCamera;
    public Animator MyAnimator;
    public GunController GunController;
    public Inventory Inventory;
    public Rigging Rigging;
    [SerializeField] private CharacterController _myController;


    [Header("이동 관련 변수")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;

    public PlayerStateMachine Machine;

    private void Awake()
    {
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

}
