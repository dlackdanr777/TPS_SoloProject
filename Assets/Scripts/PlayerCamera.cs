using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject _targetCharacter;
    [SerializeField] private Transform _zoomPos;
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

    }

    private float _mouseX;
    private float _mouseY;
    private void CameraRotate()
    {
        if (!PopupUIManager.Instance.PopupEnable)
        {
            _mouseX += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;
            _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime;

            _mouseY = Mathf.Clamp(_mouseY, _minXRotateClamp, _maxXRotateClamp);
            transform.localEulerAngles = new Vector3(-_mouseY, _mouseX, 0);
        }
    }

    public void CameraCorrection()
    {
        RaycastHit hit;
        Vector3 dir = (_mainCamera.transform.position - transform.position).normalized;

        Debug.DrawRay(transform.position, dir * _cameraDistance, Color.yellow);

        if (Physics.Raycast(transform.position, dir, out hit, _cameraDistance))
        {
            Vector3 hitPos = hit.point;
            _mainCamera.transform.position = hitPos;
        }
        else
        {
            _mainCamera.transform.localPosition = _tempCameraPos;
        }
    }

    public bool ZoomIn()
    {
        if (Vector3.Distance(_mainCamera.transform.position, _zoomPos.position) > 0.05f)
        {
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _zoomPos.position, Time.deltaTime * 10f);
            return false;
        }

        return true;
    }

    public bool ZoomOut()
    {
        if (Vector3.Distance(_mainCamera.transform.localPosition, _tempCameraPos) > 0.05f)
        {
            _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _tempCameraPos, Time.deltaTime * 10f);
            return false;
        }
        return true;
    }

}
