using UnityEngine;


/// <summary> ������ Ŭ���� </summary>
public class FlashLight : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject[] _flashLights;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _flashClip;

    public KeyCode InputKey = KeyCode.F;
    private bool _isEnable;


    /// <summary> �������� Ű�Է����� �����ϴ� �Լ� </summary>
    public void ControllFlash()
    {
        if (!Input.GetKeyDown(InputKey))
            return;

        if (_isEnable)
            DisableFlash();
        else
            EnableFlash();

        _audioSource.PlayOneShot(_flashClip);

    }


    private void EnableFlash()
    {
        foreach (GameObject light in _flashLights)
            light.SetActive(true);

        _isEnable = true;
    }


    private void DisableFlash()
    {
        foreach (GameObject light in _flashLights)
            light.SetActive(false);

        _isEnable = false;
    }
}
