using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    public enum ZombieSoundType
    {
        Idle,
        Detection,
        Tracking,
        Dead,
        Length
    }

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _idleClips;
    [SerializeField] private AudioClip[] _detectionClips;
    [SerializeField] private AudioClip[] _trackingClips;
    [SerializeField] private AudioClip[] _deadClips;

    /// <summary>
    /// 사운드를 실행하는 함수
    /// </summary>
    /// <param name="type"></param>
    public void PlayZombieSoundClip(ZombieSoundType type)
    {
        switch ((int)type)
        {
            case (int)ZombieSoundType.Idle:
                _source.clip = _idleClips[Random.Range(0, _idleClips.Length)];
                break;
            case (int)ZombieSoundType.Detection:
                _source.clip = _detectionClips[Random.Range(0, _detectionClips.Length)];
                break;
            case (int)ZombieSoundType.Tracking:
                _source.clip = _trackingClips[Random.Range(0, _trackingClips.Length)];
                break;
            case (int)ZombieSoundType.Dead:
                _source.clip = _deadClips[Random.Range(0, _deadClips.Length)];
                break;
        }
        
        _source.Play();
    }

    public void StopZombieSound()
    {
        _source.Stop();
    }
}
