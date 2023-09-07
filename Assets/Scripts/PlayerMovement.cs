using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovement
{
    public float RunSpeedMul => _runSpeedMul;

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Camera _mainCamera;

    [Header("이동 관련 변수")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeedMul;
    [SerializeField] private float _rotateSpeed;


    public void Movement(float horizontalInput, float verticalInput, float moveSpeedMul)
    {
        Vector3 moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;
        moveDir = transform.TransformDirection(moveDir) * _moveSpeed * moveSpeedMul;
        _controller.Move(moveDir * Time.deltaTime);
    }

    public void GravityEnable() //중력을 활성화시키는 함수
    {
        _controller.Move(new Vector3(0, Physics.gravity.y * 0.7f, 0) * Time.deltaTime);
    }

    public void Rotate()
    {
        Vector3 cameraRotation = new Vector3(0, _mainCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cameraRotation), Time.deltaTime * _rotateSpeed);
    }




}
