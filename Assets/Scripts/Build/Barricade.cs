using System;
using System.Collections;
using UnityEngine;


public class Barricade : MonoBehaviour, IHp
{
    public float Hp
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
            OnHpChanged?.Invoke(value);
            if (_hp == _maxHp)
                OnHpMax?.Invoke();
            else if (_hp == _minHp)
                OnHpMin?.Invoke();
        }
    }

    public float MaxHp => _maxHp;

    public float MinHp => _minHp;

    public event Action<float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;

    private float _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;
    [SerializeField] private GameObject _debrisPrefub;

    private bool _isBroken;
    public void Start()
    {
        OnHpMin += DestroyObject;
    }

    public void OnEnable()
    {
        Hp = _maxHp;
        _isBroken = false;
    }

    private void DestroyObject()
    {
        if (!_isBroken)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            _isBroken = true;
            Destroy(gameObject);
        }

    }

    public void DepleteHp(object subject, float value)
    {
        Hp -= value;
        Debug.Log("����" + Hp);
        OnHpDepleted?.Invoke(subject, value);
    }

    public void RecoverHp(object subject, float value)
    {
        OnHpRecoverd?.Invoke(subject, value);
    }
}
