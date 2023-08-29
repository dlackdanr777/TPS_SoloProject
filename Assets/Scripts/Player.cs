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

    private bool _runEnable;

    private void Awake()
    {
        _myController = GetComponent<CharacterController>();
        _myAnimator = GetComponent<Animator>();
        _myCamera = Camera.main;
    }

    private void Update()
    {
        Movement();
        PlayerRotate();
    }



    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _runEnable = Input.GetKey(KeyCode.LeftShift) && vertical > 0.1f;

        Vector3 horizontalMoveDir = new Vector3(horizontal, 0, 0);
        horizontalMoveDir = transform.TransformDirection(horizontalMoveDir).normalized;
        horizontalMoveDir *= _horizontalSpeed;
        _myController.Move(horizontalMoveDir * Time.deltaTime);

        Vector3 verticalMoveDir = new Vector3(0, 0, vertical);
        verticalMoveDir = transform.TransformDirection(verticalMoveDir);
        verticalMoveDir *= _verticalSpeed;
        if (_runEnable) { verticalMoveDir *= _runSpeedMul; }
        _myController.Move(verticalMoveDir * Time.deltaTime);

        //_myAnimator.SetFloat("Horizontal", horizontal);
        _myAnimator.SetFloat("Vertical", vertical);
        _myAnimator.SetBool("IsRun", _runEnable);
    }


    private void PlayerRotate()
    {
        Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
    }
}
