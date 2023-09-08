using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHp
{
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
    public event Action onHpMax;
    public event Action onHpMin;

    private float _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;

    public void Start()
    {
        Init();
 
    }
    public void Init()
    {
        _hp = _maxHp;
        onHpMin = () => Destroy(gameObject);
    }

    public void RecoverHp(object subject, float value)
    {
        hp += value;
        OnHpRecoverd?.Invoke(subject, value);
    }

    public void DepleteHp(object subject, float value)
    {
        hp -= value;
        Debug.Log("피격되었습니다! 남은체력:" +_hp);
        OnHpDepleted?.Invoke(subject, value);
    }
}
