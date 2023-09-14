using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    public enum ZombieSoundType
    {
        Idle,
        detection,
        Tracking,
        Dead,
        Length
    }

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;


    /// <summary>
    /// ���带 �����ϴ� �Լ�
    /// </summary>
    /// <param name="type"></param>
    public void PlayZombieSoundClip(ZombieSoundType type)
    {
        _source.clip = _clips[(int)type];
        _source.Play();
    }

    public void StopZombieSound()
    {
        _source.Stop();
    }
}
