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

    public void GravityEnable() //�߷��� Ȱ��ȭ��Ű�� �Լ�
    {
        if(_controller.enabled)
            _controller.Move(new Vector3(0, Physics.gravity.y * 0.5f, 0) * Time.deltaTime);
    }
}
