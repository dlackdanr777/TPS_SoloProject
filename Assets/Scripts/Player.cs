using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _myController;
    [HideInInspector] public Animator MyAnimator;
    private Camera _myCamera;

    [Header("오브젝트 넣는 곳")]
    [SerializeField] private GameObject _centerCamera;
    [SerializeField] private Transform _crossHair;
    [SerializeField] private Transform _muzzle;

    [Header("이동 관련 변수")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;

    public bool AimMode = false;
    public bool IsAimModeChanged;
    public bool IsFire;
    public bool IsReload;

    private float _horizontalInput;
    private float _verticalInput;
    public bool RunEnable;

    private IState _currentState;
    private IState _idleState;
    private IState _movementState;
    private IState _aimModeState;

    private void Awake()
    {
        _myController = GetComponent<CharacterController>();
        MyAnimator = GetComponent<Animator>();
        _myCamera = Camera.main;
    }


    private void Update()
    {
        Movement();
        PlayerRotate();
        PostionCrossHair();
        Debug.DrawRay(_muzzle.position, _muzzle.forward * 100, Color.red);
    }

    private void ComponentInit()
    {

    }

    private void StateInit()
    {
        _idleState = new IdleState(this);
        _movementState = new MovementState(this);
        _aimModeState = new AimModeState(this);
    }

    public void Movement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        RunEnable = Input.GetKey(KeyCode.LeftShift) && _verticalInput > 0.1f && !AimMode && !IsReload;

        Vector3 moveDir = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
        moveDir = transform.TransformDirection(moveDir) * _moveSpeed;
        if( RunEnable ) { moveDir *= _runSpeedMul; }
        moveDir.y = Physics.gravity.y * 0.6f;

        _myController.Move(moveDir * Time.deltaTime);

        MyAnimator.SetFloat("Vertical", _verticalInput);
        MyAnimator.SetFloat("Horizontal", _horizontalInput);
        MyAnimator.SetBool("IsRun", RunEnable);
    }

    public void PlayerRotate() //캐릭터를 마우스회전에 따라 회전시키는 함수
    {
        if (_horizontalInput != 0 || _verticalInput != 0 || AimMode)
        {
            Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
        }
    }

    private void PostionCrossHair()
    {
        if (AimMode)
        {
            if (!_crossHair.gameObject.activeSelf)
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
        else
        {
            if(_crossHair.gameObject.activeSelf)
                _crossHair.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if(!IsAimModeChanged && !RunEnable)
        {
            if (Input.GetMouseButton(1) && !AimMode)
            {
                StartCoroutine(AimModeStart());
            }

            else if (Input.GetMouseButtonUp(1) && AimMode)
            {
                StartCoroutine(AimModeEnd());
            }
        }
    }

    private IEnumerator AimModeStart()
    {

        AimMode = true;
        IsAimModeChanged = true;
        MyAnimator.SetBool("AimMode", true);
        yield return StartCoroutine(_centerCamera.GetComponent<PlayerCamera>().CameraAimModeStart());
        IsAimModeChanged = false;
    }

    private IEnumerator AimModeEnd()
    {

        AimMode = false;
        IsAimModeChanged = true;
        MyAnimator.SetBool("AimMode", false);
        yield return StartCoroutine(_centerCamera.GetComponent<PlayerCamera>().CameraAimModeEnd());
        IsAimModeChanged = false;
    }

    public void StartTryFireAnime(bool value)
    {
        IsFire = value;
        //MyAnimator.SetBool("Fire", value);
    }
}
