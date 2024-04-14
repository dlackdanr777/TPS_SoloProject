using UnityEngine;

/// <summary> ���� �Ҹ��� ���õ� Ŭ���� </summary>
public class EnemySounds : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _idleClips;
    [SerializeField] private AudioClip[] _detectionClips;
    [SerializeField] private AudioClip[] _trackingClips;
    [SerializeField] private AudioClip[] _attackClips;
    [SerializeField] private AudioClip[] _deadClips;


    /// <summary>���带 �����ϴ� �Լ�</summary>
    public void PlayZombieSoundClip(EnemySoundType type)
    {
        switch ((int)type)
        {
            case (int)EnemySoundType.Idle:
                _source.clip = _idleClips[Random.Range(0, _idleClips.Length)];
                break;
            case (int)EnemySoundType.Detection:
                _source.clip = _detectionClips[Random.Range(0, _detectionClips.Length)];
                break;
            case (int)EnemySoundType.Tracking:
                _source.clip = _trackingClips[Random.Range(0, _trackingClips.Length)];
                break;
            case (int)EnemySoundType.Attack:
                _source.clip = _attackClips[Random.Range(0, _attackClips.Length)];
                break;
            case (int)EnemySoundType.Dead:
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
