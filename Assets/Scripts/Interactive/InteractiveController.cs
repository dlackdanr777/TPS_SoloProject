using UnityEngine;


/// <summary> IInteractive interface를 가진 오브젝트를 조작하는 클래스 </summary>
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


    /// <summary> Ray를 쏴 상호작용이 가능한 물체를 감지하는 함수 </summary>
    private void CheckInteractive()
    {
        _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (!Physics.Raycast(_ray, out _hit, _rayDistance, _layerMask))
        {
            if (_interactive == null)
                return;

            _interactive.DisableInteraction();
            _interactive = null;
            return;
        }

        if (_interactive != null)
            return;

        if (_hit.transform.GetComponent<Iinteractive>() == null)
            return;

        _interactive = _hit.transform.GetComponent<Iinteractive>();
        _interactive.EnableInteraction();
    }


    /// <summary> 키를 입력해 상호작용을 하는 함수 </summary>
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
