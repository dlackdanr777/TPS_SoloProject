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

    [SerializeField] private float _moveSpeed;
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
        Attack();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _runEnable = Input.GetKey(KeyCode.LeftShift);

        _myAnimator.SetFloat("Vertical", vertical);
        _myAnimator.SetFloat("SpeedMul", _moveSpeed);
        _myAnimator.SetBool("IsRun", _runEnable);
    }

    private void Attack()
    {
        _myAnimator.SetBool("AttackReddy", Input.GetKey(KeyCode.Mouse1));
    }

    private void PlayerRotate()
    {
        Vector3 cameraRotation = new Vector3(0, _myCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
    }
}
