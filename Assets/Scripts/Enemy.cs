using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHp
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;

    
    public float hp
    {
        get => _hp;
        private set
        {
            if (_hp == value)
                return;

            if (value > _maxHp)
                value = _maxHp;
            else if (value < _minHp)
                value = _minHp;

            _hp = value;
            onHpChanged?.Invoke(value);
            if (_hp == _maxHp)
                onHpMax?.Invoke();
            else if (_hp == _minHp)
                onHpMin?.Invoke();
        }
    }
    public float maxHp => _maxHp;
    public float minHp => _minHp;

    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action<Vector3> OnRotatedHandler;
    public event Action onHpMax;
    public event Action onHpMin;

    public Action OnTargetFollowedHandler;

    private float _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;
    [SerializeField] private float _rotateSpeed;
    private bool _isDead;

    private void Start()
    {
        Init();
        ActionInit();
    }

    private void FixedUpdate()
    {
        if (!_isDead)
        {
            //OnRotatedHandler(PathFinding.PathfindingFirstPos);
        }
    }

    private void OnEnable()
    {
        Init();
    }


    private void Init()
    {
        _hp = _maxHp;
        _isDead = false;
        _controller.enabled = true;
    }

    private void ActionInit()
    {
        onHpMin = () =>
        {
            if (!_isDead)
            {
                StartCoroutine(ObjectPoolManager.Instance.ZombleDisable(gameObject));
                _controller.enabled = false;
                _animator.SetTrigger("Dead");
                _isDead = true;
            }
        };
        OnRotatedHandler = Rotate;
    }

    private void Rotate(Vector3 targetPos)
    {
        Vector3 thisPos = transform.position;
        targetPos.y = 0f;
        thisPos.y = 0f;

        Vector3 dir = targetPos - thisPos;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSpeed);
    }

    public void RecoverHp(object subject, float value)
    {
        if (!_isDead)
        {
            hp += value;
            OnHpRecoverd?.Invoke(subject, value);
        }
    }

    public void DepleteHp(object subject, float value)
    {
        if (!_isDead)
        {
            hp -= value;
            Debug.Log("피격되었습니다! 남은체력:" + _hp);
            OnHpDepleted?.Invoke(subject, value);
        }
    }
}
