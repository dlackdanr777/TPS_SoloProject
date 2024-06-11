using UnityEngine;


/// <summary> IInteractive interface�� ���� ������Ʈ�� �����ϴ� Ŭ���� </summary>
public class InteractiveController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _layerMask;
    
    private Ray _ray;
    private RaycastHit _hit;
    private Iinteractive _interactive;


    private void Update()
    {
        CheckInteractive();
        InputInteractive();
    }


    /// <summary> Ray�� �� ��ȣ�ۿ��� ������ ��ü�� �����ϴ� �Լ� </summary>
    private void CheckInteractive()
    {
        _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        // ī�޶� ������ ���̸� ���.
        //���� hit�� ������Ʈ�� ���� ���
        if (!Physics.Raycast(_ray, out _hit, _rayDistance, _layerMask))
        {
            //������ null�ϰ�� ����
            if (_interactive == null)
                return;

            //�ƴҰ�� ��Ȱ��ȭ �Լ��� ������ null�� ����
            _interactive.DisableInteraction();
            _interactive = null;
            return;
        }

        if (_interactive != null)
            return;

        if (_hit.transform.GetComponent<Iinteractive>() == null)
            return;

        //�ش� ������Ʈ���� Iinteractive �������̽��� ������ Ȱ��ȭ �Լ� ����
        _interactive = _hit.transform.GetComponent<Iinteractive>();
        _interactive.EnableInteraction();
    }


    /// <summary> Ű�� �Է��� ��ȣ�ۿ��� �ϴ� �Լ� </summary>
    private void InputInteractive()
    {
        if (_interactive == null)
            return;

        if (!Input.GetKeyDown(_interactive.InputKey))
            return;

            _interactive.Interact();
            _interactive.DisableInteraction();
            _interactive = null;
    }
}
