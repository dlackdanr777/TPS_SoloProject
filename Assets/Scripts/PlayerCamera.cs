using System.Collections;
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
        _aimModeCameraPos = new Vector3(0.7f, 0.26f, -1f);
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
        if (!_player._aimMode)
        {
            Vector3 dir = (_mainCamera.transform.position - transform.position).normalized;
            Debug.DrawRay(transform.position, dir * _cameraDistance, Color.yellow);

            if (Physics.Raycast(transform.position, dir, out hit, _cameraDistance))
            {
                _mainCamera.transform.position = hit.point;
            }
            else
            {
                _mainCamera.transform.localPosition = _tempCameraPos;
            }
        }
    }

    private Coroutine CameraAimModeStartRoutine;
    private Coroutine CameraAimModeEndRoutine;


    public void CameraAimModeStartFunc()
    {
        if(CameraAimModeStartRoutine != null)
        {StopCoroutine(CameraAimModeStartRoutine);}

        if (CameraAimModeEndRoutine != null)
        { StopCoroutine(CameraAimModeEndRoutine); }

        CameraAimModeStartRoutine = StartCoroutine(CameraAimModeStart());
    }

    public void CameraAimModeEndFunc()
    {
        if (CameraAimModeStartRoutine != null)
        { StopCoroutine(CameraAimModeStartRoutine); }

        if (CameraAimModeEndRoutine != null)
        { StopCoroutine(CameraAimModeEndRoutine); }

        CameraAimModeEndRoutine = StartCoroutine(CameraAimModeEnd());
    }

    private IEnumerator CameraAimModeStart()
    {
        while(Vector3.Distance(_mainCamera.transform.localPosition, _aimModeCameraPos) > 0.5f)
        {
            Debug.Log("스타트");
            _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _aimModeCameraPos, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator CameraAimModeEnd()
    {
        while (Vector3.Distance(_mainCamera.transform.localPosition, _tempCameraPos) > 0.5f)
        {
            Debug.Log("엔드");
            _mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _tempCameraPos, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
