using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public KeyCode InputKey = KeyCode.F;

    [SerializeField] private GameObject[] _flashLights;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _flashClip;

    private bool _isEnable;


    public void ControllFlash()
    {
        if (Input.GetKeyDown(InputKey))
        {
            if (_isEnable)
                DisableFlash();
            else
                EnableFlash();

            _audioSource.PlayOneShot(_flashClip);
        }
    }


    private void EnableFlash()
    {
        foreach (var light in _flashLights)
            light.SetActive(true);
        _isEnable = true;
    }

    private void DisableFlash()
    {
        foreach (var light in _flashLights)
            light.SetActive(false);
        _isEnable = false;
    }
}
