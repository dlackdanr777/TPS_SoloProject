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

    [Header("������Ʈ �ִ� ��")]
    [SerializeField] private GameObject _centerCamera;
    [SerializeField] private Transform _crossHair;
    [SerializeField] private Transform _muzzle;

    [Header("�̵� ���� ����")]
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;

    [Header("���� ���� ����")]
    [SerializeField] private float _rpm; //�д� �߻�ӵ�
    private float _fireInterval; //�� �ʸ��� �����ϳ�?
    [SerializeField]  private int _maxAmmo; //�ִ� ��ź��
    private int _ammo; //���� ��ź��

    public bool _aimMode;
    private bool isAimModeChanged;

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

    private void Start()
    {
        _fireInterval = 1 / (_rpm / 60);
        _ammo = _maxAmmo;
    }

    private void Update()
    {
        Movement();
        PlayerRotate();
        PostionCrossHair();
        Fire();
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

    private void PlayerRotate()
    {
        if (_horizontalInput != 0 || _verticalInput != 0 || _aimMode)
        {
            Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
        }
    }

    float _fireTimer;
    private void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            _fireTimer += Time.deltaTime;

            if (_fireTimer > _fireInterval)
            {
                _fireTimer = 0;
                if (_ammo > 0)
                {
                    _ammo--;
                    Debug.Log("�߻�");
                }
                else
                {
                    Debug.Log("������ �ʿ�");
                }
            }
        }
    }

    private void PostionCrossHair()
    {
        RaycastHit hit;
        Ray ray = _myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //ī�޶� ���߾ӿ� ���̸� ��ġ��Ų��
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
            _myAnimator.SetBool("AimMode", true);
            StartCoroutine(AimModeStart());
        }

        else if (Input.GetMouseButtonUp(1))
        {

            StartCoroutine(AimModeEnd());
        }
        if (_aimMode)
        {
            _spine.LookAt(_crossHair.position);
            _spine.rotation = _spine.rotation * Quaternion.Euler(new Vector3(0, 109.15f, -67.3f));
        }
    }

    private IEnumerator AimModeStart()
    {
        if(!_aimMode && !isAimModeChanged)
        {
            isAimModeChanged = true;
            _aimMode = true;
            yield return StartCoroutine(_centerCamera.GetComponent<PlayerCamera>().CameraAimModeStart());
            isAimModeChanged = false;
        }
    }

    private IEnumerator AimModeEnd()
    {
        if(_aimMode && !isAimModeChanged)
        {
            isAimModeChanged = true;
            yield return StartCoroutine(_centerCamera.GetComponent<PlayerCamera>().CameraAimModeEnd());
            _aimMode = false;
            _myAnimator.SetBool("AimMode", false);
            isAimModeChanged = false;
        }
    }

}
