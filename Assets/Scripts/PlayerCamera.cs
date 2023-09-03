using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Cursor = UnityEngine.Cursor;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject _targetCharacter;
    [SerializeField] private Transform _crossHair;
    [SerializeField] private Transform _zoomPos;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _tempPos = transform.position;
        _tempCameraPos = _mainCamera.transform.localPosition;
        _aimModeCameraPos = new Vector3(0.65f, 0.45f, -0.8f);
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
    }

    public void CrossHairEnable() //크로스헤어를 활성화시키는 함수
    {
        if (!_crossHair.gameObject.activeSelf)
            _crossHair.gameObject.SetActive(true);

        RaycastHit hit;
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //카메라 정중앙에 레이를 위치시킨다
        float distance = 50f;
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Vector3 hitPos = hit.point;
            float hitDistance = Vector3.Distance(_mainCamera.transform.position, hit.point);
            _crossHair.position = hitPos;
            _crossHair.localScale = Vector3.one * (hitDistance / distance);
            _crossHair.LookAt(_mainCamera.transform.position);

        }
        else
        {
            _crossHair.position = _mainCamera.transform.position + _mainCamera.transform.forward * distance;
            _crossHair.localScale = Vector3.one;
            _crossHair.LookAt(_mainCamera.transform.position);
        }
    }

    public void CrossHairDisable() //크로스헤어를 비활성화 시키는 함수
    {
        _crossHair.gameObject.SetActive(false);
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
