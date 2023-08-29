using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _myController;
    private Animator _myAnimator;
    private Camera _myCamera;

    [SerializeField] private GameObject _centerCamera;

    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;


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

        Debug.DrawRay(_muzzle.position, _muzzle.forward * 100, Color.red);
    }



    private void Movement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _runEnable = Input.GetKey(KeyCode.LeftShift) && _verticalInput > 0.1f;

        Vector3 horizontalMoveDir = new Vector3(_horizontalInput, 0, 0);
        horizontalMoveDir = transform.TransformDirection(horizontalMoveDir).normalized;
        horizontalMoveDir *= _horizontalSpeed;
        horizontalMoveDir.y = Physics.gravity.y;
        _myController.Move(horizontalMoveDir * Time.deltaTime);

        Vector3 verticalMoveDir = new Vector3(0, 0, _verticalInput);
        verticalMoveDir = transform.TransformDirection(verticalMoveDir);
        verticalMoveDir *= _verticalSpeed;
        if (_runEnable) { verticalMoveDir *= _runSpeedMul; }
        _myController.Move(verticalMoveDir * Time.deltaTime);

        //_myAnimator.SetFloat("Horizontal", horizontal);
        _myAnimator.SetFloat("Vertical", _verticalInput);
        _myAnimator.SetBool("IsRun", _runEnable);
    }

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _muzzle;
    private Transform _spine;
    public bool _aimMode;


    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _myAnimator.SetBool("AimMode", true);
            StartCoroutine(AimModeStart());
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _myAnimator.SetBool("AimMode", false);
            StartCoroutine(AimModeEnd());
        }
        if (_aimMode)
        {
            _spine.LookAt(_target.position);
            _spine.rotation = _spine.rotation * Quaternion.Euler(new Vector3(0, 108.77f, -66.17f));
        }
    }

    private IEnumerator AimModeStart()
    {
        _aimMode = true;
        _centerCamera.GetComponent<PlayerCamera>().CameraAimModeStartFunc();
        yield return new WaitForSeconds(0.02f);

    }

    private IEnumerator AimModeEnd()
    {
        _centerCamera.GetComponent<PlayerCamera>().CameraAimModeEndFunc();
        yield return new WaitForSeconds(1f);
        _aimMode = false;

    }


    private void PlayerRotate()
    {
        if(_horizontalInput != 0 || _verticalInput != 0 || _aimMode)
        {
            Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
        }
    }
}
