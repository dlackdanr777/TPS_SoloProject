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

        _player.MyAnimator.SetFloat("MouseY", _mouseY);
    }


    private void CameraCorrection()
    {
        if (!_player.AimMode && !_player.IsAimModeChanged)
        {
            RaycastHit hit;
            Vector3 dir = (_mainCamera.transform.position - transform.position).normalized; //ī ޶  ߽        ī ޶            ִ´ .

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
    }


    private bool isRoutineStart = false;
    public IEnumerator CameraAimModeStart()
    {
        if (!isRoutineStart)
        {
            isRoutineStart = true;
            while (Vector3.Distance(_mainCamera.transform.localPosition, _aimModeCameraPos) > 0.1f)
            {
                _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _aimModeCameraPos, 0.15f);
                yield return new WaitForSeconds(0.01f);
            }
            isRoutineStart = false;
        }
    }

    public IEnumerator CameraAimModeEnd()
    {
        if (!isRoutineStart)
        {
            isRoutineStart = true;
            while (Vector3.Distance(_mainCamera.transform.localPosition, _tempCameraPos) > 0.1f)
            {
                _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _tempCameraPos, 0.15f);
                yield return new WaitForSeconds(0.01f);
            }
            isRoutineStart = false;
        }
    }
}
