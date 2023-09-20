using Cinemachine;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Camera _brainCamera;
    [SerializeField] private CinemachineVirtualCamera _mainVitualCamera;
    [SerializeField] private CinemachineVirtualCamera _zoomVitualCamera;
    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minYRotateClamp;
    [SerializeField] private float _maxYRotateClamp;

    [Header("카메라 흔들림")]
    [SerializeField] private float _shakeAmplitude = 1.2f;
    [SerializeField] private float _shakeFrequency = 2.0f;
    [SerializeField] private float _shakeDuration = 0.1f;
    private float _shakeTime;

    private Vector3 _tempPos;

    private void Start()
    {
        if (_virtualCameraNoise == null)
            _virtualCameraNoise = _zoomVitualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.IsGameEnd)
        {
            transform.position = _target.transform.position;
            CameraShake();
            if (!PopupUIManager.Instance.PopupEnable)
            {
                CameraRotate();
            }
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
            _mainVitualCamera.gameObject.SetActive(false);
            _zoomVitualCamera.gameObject.SetActive(true);
            return false;
        }
        return true;

    }

    public bool ZoomOut()
    {
        if (Vector3.Distance(_brainCamera.transform.position, _mainVitualCamera.transform.position) > 0.05f)
          {
            _mainVitualCamera.gameObject.SetActive(true);
            _zoomVitualCamera.gameObject.SetActive(false);
            return false;
        }
        return true;
    }

    public void CameraShakeStart()
    {
        _shakeTime = _shakeDuration;
    }

    private void CameraShake()
    {
        if(_virtualCameraNoise != null || _zoomVitualCamera != null)
        {
            if(_shakeTime > 0)
            {
                _virtualCameraNoise.m_AmplitudeGain = _shakeAmplitude;
                _virtualCameraNoise.m_FrequencyGain = _shakeFrequency;

                _shakeTime -= Time.deltaTime;
            }
            else
            {
                _virtualCameraNoise.m_AmplitudeGain = 0f;
                _shakeTime = 0f;
            }
        }

    }

}
