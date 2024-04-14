using System;
using System.Collections;
using UnityEngine;


public class Barricade : MonoBehaviour, IHp
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

    public float MaxHp => _maxHp;

    public float MinHp => _minHp;

    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action onHpMax;
    public event Action onHpMin;

    private float _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;
    [SerializeField] private GameObject _debrisPrefub;

    private bool _isBroken;
    public void Start()
    {
        onHpMin += DestroyObject;
    }

    public void OnEnable()
    {
        hp = _maxHp;
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
        hp -= value;
        Debug.Log("¸ÂÀ½" + hp);
        OnHpDepleted?.Invoke(subject, value);
    }

    public void RecoverHp(object subject, float value)
    {
        OnHpRecoverd?.Invoke(subject, value);
        throw new NotImplementedException();
    }
}
