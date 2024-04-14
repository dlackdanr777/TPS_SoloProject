using Cinemachine;
using UnityEngine;


/// <summary> �÷��̾ ī�޶� ���콺�� ������ �� �ְ� �ϴ� Ŭ���� </summary>
public class CinemachineCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _target;
    [SerializeField] private Camera _brainCamera;
    [SerializeField] private CinemachineVirtualCamera _mainVitualCamera;
    [SerializeField] private CinemachineVirtualCamera _zoomVitualCamera;


    [Space]
    [Header("Option")]
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minYRotateClamp;
    [SerializeField] private float _maxYRotateClamp;
    [SerializeField] private float _shakeAmplitude = 1.2f;
    [SerializeField] private float _shakeFrequency = 2.0f;
    [SerializeField] private float _shakeDuration = 0.1f;


    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;
    private Vector3 _tempPos;
    private float _shakeTime;
    private float _mouseX;
    private float _mouseY;


    private void Start()
    {
        if (_virtualCameraNoise == null)
            _virtualCameraNoise = _zoomVitualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    private void LateUpdate()
    {
        if (GameManager.Instance.IsGameEnd)
            return;


        transform.position = _target.transform.position;
        CameraShake();
        if (!PopupUIManager.PopupEnable)
        {
            CameraRotate();
        }
    }


    /// <summary> ī�޶� ȸ�� </summary>
    private void CameraRotate()
    {
        _mouseX += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;
        _mouseY += Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime;

        _mouseY = Mathf.Clamp(_mouseY, _minYRotateClamp, _maxYRotateClamp);
        transform.localEulerAngles = new Vector3(-_mouseY, _mouseX, 0);
    }


    /// <summary> �� �� ��� </summary>
    public bool ZoomIn()
    {
        if (Vector3.Distance(_brainCamera.transform.position, _zoomVitualCamera.transform.position) <= 0.05f)
            return false;

        _mainVitualCamera.gameObject.SetActive(false);
        _zoomVitualCamera.gameObject.SetActive(true);
        return false;

    }


    /// <summary> �� �ƿ� ��� </summary>
    public bool ZoomOut()
    {
        if (Vector3.Distance(_brainCamera.transform.position, _mainVitualCamera.transform.position) <= 0.05f)
            return true;

        _mainVitualCamera.gameObject.SetActive(true);
        _zoomVitualCamera.gameObject.SetActive(false);
        return false;
    }


    /// <summary> ī�޶� ��鸲 ����� �����ϴ� �Լ� </summary>
    public void CameraShakeStart()
    {
        _shakeTime = _shakeDuration;
    }


    /// <summary> CameraShakeStart�Լ��� ����Ǹ� ī�޶� ��鸲�� ���������� �ִ� �Լ� </summary>
    private void CameraShake()
    {
        if (_virtualCameraNoise == null || _zoomVitualCamera == null)
            return;

        if (_shakeTime > 0)
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
