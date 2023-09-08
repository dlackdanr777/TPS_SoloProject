using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Gravity : MonoBehaviour
{
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        GravityEnable();
    }

    public void GravityEnable() //중력을 활성화시키는 함수
    {
        _controller.Move(new Vector3(0, Physics.gravity.y * 0.7f, 0) * Time.deltaTime);
    }
}
