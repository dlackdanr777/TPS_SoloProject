using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _myController;
    private Animator _myAnimator;
    private Camera _myCamera;
    private Transform _spine;

    [Header("오브젝트 넣는 곳")]
    [SerializeField] private GameObject _centerCamera;
    [SerializeField] private Transform _crossHair;
    [SerializeField] private Transform _muzzle;

    [Header("이동 관련 변수")]
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;

    public bool AimMode = false;
    private bool _isFire;
    private bool _isAimModeChanged;
    

    private float _horizontalInput;
    private float _verticalInput;
    private bool _runEnable;

    private void Awake()
    {
        _myController = GetComponent<CharacterController>();
        _myAnimator = GetComponent<Animator>();
        _myCamera = Camera.main;
        _spine = _myAnimator.GetBoneTransform(HumanBodyBones.Spine);
    }


    private void Update()
    {
        Movement();
        PlayerRotate();
        PostionCrossHair();
        Debug.DrawRay(_muzzle.position, _muzzle.forward * 100, Color.red);
    }


    private void Movement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _runEnable = Input.GetKey(KeyCode.LeftShift) && _verticalInput > 0.1f && !AimMode;

        Vector3 horizontalMoveDir = new Vector3(_horizontalInput, 0, 0);
        horizontalMoveDir = transform.TransformDirection(horizontalMoveDir).normalized;
        horizontalMoveDir *= _horizontalSpeed;
        if (_verticalInput < 0)
        { horizontalMoveDir *= 0.3f; }
        horizontalMoveDir.y = Physics.gravity.y;
        _myController.Move(horizontalMoveDir * Time.deltaTime);

        Vector3 verticalMoveDir = new Vector3(0, 0, _verticalInput);
        verticalMoveDir = transform.TransformDirection(verticalMoveDir);
        verticalMoveDir *= _verticalSpeed;
        if (_verticalInput < 0)
        { verticalMoveDir *= 0.8f; }
        if (_runEnable) { verticalMoveDir *= _runSpeedMul; }
        _myController.Move(verticalMoveDir * Time.deltaTime);

        //_myAnimator.SetFloat("Horizontal", horizontal);
        _myAnimator.SetFloat("Vertical", _verticalInput);
        _myAnimator.SetFloat("Horizontal", _horizontalInput);
        _myAnimator.SetBool("IsRun", _runEnable);
    }

    private void PlayerRotate()
    {
        if (_horizontalInput != 0 || _verticalInput != 0 || AimMode)
        {
            Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
        }
    }

    private void PostionCrossHair()
    {
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

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            StartCoroutine(AimModeStart());
        }

        else if (Input.GetMouseButtonUp(1))
        {

            StartCoroutine(AimModeEnd());
        }
        if (AimMode)
        {
            _spine.LookAt(_crossHair.position);
            _spine.rotation = _spine.rotation * Quaternion.Euler(new Vector3(0, 109.15f, -67.3f));
        }
    }

    private IEnumerator AimModeStart()
    {
        if(!AimMode && !_isAimModeChanged && !_runEnable)
        {
            _myAnimator.SetBool("AimMode", true);
            _isAimModeChanged = true;
            AimMode = true;
            yield return StartCoroutine(_centerCamera.GetComponent<PlayerCamera>().CameraAimModeStart());
            _isAimModeChanged = false;
        }
    }

    private IEnumerator AimModeEnd()
    {
        if(AimMode && !_isAimModeChanged)
        {
            _isAimModeChanged = true;
            yield return StartCoroutine(_centerCamera.GetComponent<PlayerCamera>().CameraAimModeEnd());
            AimMode = false;
            _myAnimator.SetBool("AimMode", false);
            _isAimModeChanged = false;
        }
    }

    public void StartTryFireAnime(bool value)
    {
        _isFire = value;
        _myAnimator.SetBool("Fire", value);
    }
}
