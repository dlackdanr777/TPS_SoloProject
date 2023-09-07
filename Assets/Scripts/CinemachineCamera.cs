using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Camera _brainCamera;
    [SerializeField] private GameObject _mainVitualCamera;
    [SerializeField] private GameObject _zoomVitualCamera;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private float _minYRotateClamp;
    [SerializeField] private float _maxYRotateClamp;

    private Vector3 _tempPos;

    private void Start()
    {
        _tempPos = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = _tempPos + _target.transform.position;
        if (!PopupUIManager.Instance.PopupEnable)
        {
            CameraRotate();
        }
    }

    private float _mouseX;
    private float _mouseY;
    private void CameraRotate()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;
        _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime;

        _mouseY = Mathf.Clamp(_mouseY, _minYRotateClamp, _maxYRotateClamp);
        transform.localEulerAngles = new Vector3(-_mouseY, _mouseX, 0);
    }


    public bool ZoomIn()
    {
        if (Vector3.Distance(_brainCamera.transform.position, _zoomVitualCamera.transform.position) > 0.05f)
          {
            _mainVitualCamera.SetActive(false);
            _zoomVitualCamera.SetActive(true);
            return false;
        }
        return true;

    }

    public bool ZoomOut()
    {
        if (Vector3.Distance(_brainCamera.transform.position, _mainVitualCamera.transform.position) > 0.05f)
          {
            _mainVitualCamera.SetActive(true);
            _zoomVitualCamera.SetActive(false);
            return false;
        }
        return true;
    }

}
