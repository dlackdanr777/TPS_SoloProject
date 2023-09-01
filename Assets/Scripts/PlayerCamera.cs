using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Cursor = UnityEngine.Cursor;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject _targetCharacter;
    private Player _player;
    private Camera _mainCamera;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private float _minXRotateClamp;
    [SerializeField] private float _maxXRotateClamp;

    private Vector3 _tempPos;
    private Vector3 _tempCameraPos;
    private Vector3 _aimModeCameraPos;
    private float _cameraDistance;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _player = _targetCharacter.GetComponent<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _tempPos = transform.position;
        _tempCameraPos = _mainCamera.transform.localPosition;
        _aimModeCameraPos = _tempCameraPos;
        _aimModeCameraPos.z = -1f;
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
        _mouseX += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;
        _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime;

        _mouseY = Mathf.Clamp(_mouseY, _minXRotateClamp, _maxXRotateClamp);
        transform.localEulerAngles = new Vector3(-_mouseY, _mouseX, 0);

        _player.MyAnimator.SetFloat("MouseY", _mouseY);
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
        if (Vector3.Distance(_mainCamera.transform.localPosition, _aimModeCameraPos) > 0.1f)
        {
            _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _aimModeCameraPos, Time.deltaTime * 10f);
            return false;
        }

        return true;
    }

    public bool ZoomOut()
    {
        if (Vector3.Distance(_mainCamera.transform.localPosition, _tempCameraPos) > 0.1f)
        {
            _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _tempCameraPos, Time.deltaTime * 10f);
            return false;
        }
        return true;
    }

}
