using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject _targetCharacter;
    private Camera _mainCamera;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private float _minXRotateClamp;
    [SerializeField] private float _maxXRotateClamp;

    private Vector3 _tempPos;
    private Vector3 _tempCameraPos;
    private float _cameraDistance;
    private void Awake()
    {
        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _tempPos = transform.position;
        _tempCameraPos = _mainCamera.transform.localPosition;
        _cameraDistance = Vector3.Distance(transform.position, _mainCamera.transform.position);
    }

    private void Update()
    {
        transform.position = _tempPos + _targetCharacter.transform.position;
        CameraRotate();
        CameraCorrection();
    }


    private float _mouseX;
    private float _mouseY;
    private void CameraRotate()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;
        _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime;

        _mouseY = Mathf.Clamp(_mouseY, _minXRotateClamp, _maxXRotateClamp);
        transform.localEulerAngles = new Vector3(-_mouseY, _mouseX, 0);
    }

    RaycastHit hit;
    private void CameraCorrection()
    {
        Vector3 dir = (_mainCamera.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, dir * _cameraDistance, Color.yellow);

        if(Physics.Raycast(transform.position, dir, out hit, _cameraDistance))
        {
            _mainCamera.transform.position = hit.point;
        }
        else
        {
            _mainCamera.transform.localPosition = _tempCameraPos;
        }
    }
}
