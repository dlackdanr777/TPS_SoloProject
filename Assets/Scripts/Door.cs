using UnityEngine;


public class Door : MonoBehaviour, Iinteractive
{
    public KeyCode InputKey => KeyCode.E;
    [SerializeField] private Animation _animation;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    [SerializeField] private AudioClip _lockClip;

    [SerializeField] private bool _isLocked;

    private bool _isOpened;

    public void Interact()
    {
        DoorControll();
    }

    public void DisableInteraction()
    {
        UIManager.Instance.HiddenRightText();
    }

    public void EnableInteraction()
    {
        if(!_isLocked)
            UIManager.Instance.ShowRightText("[E] ����/�ݱ�");
        else
            UIManager.Instance.ShowRightText("[E] ���");
    }



    private void DoorControll()
    {
        if (!_isLocked)
        {
            if (_animation["DoorWide_open"].normalizedTime == 0)
            {
                if (_isOpened)
                {
                    if (_animation["DoorWide_open"].normalizedTime > 0.98f || _animation["DoorWide_open"].normalizedTime == 0)
                        _animation["DoorWide_open"].normalizedTime = 1;
                    _animation["DoorWide_open"].speed = -1;

                    _audioSource.clip = _closeClip;
                }

                else
                {
                    _animation["DoorWide_open"].speed = 1;

                    _audioSource.clip = _openClip;
                }

                _isOpened = !_isOpened;
                _animation.Play("DoorWide_open");
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.clip = _lockClip;
            _audioSource.Play();
        }
    }
}
