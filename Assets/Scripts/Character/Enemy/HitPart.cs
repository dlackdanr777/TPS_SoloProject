using System;
using UnityEngine;

/// <summary>피격 부위</summary>
public class HitPart : MonoBehaviour, IHp
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private string _partName;
    [SerializeField] private float _multiple;

    public float hp => _enemy.hp;
    public float MaxHp => _enemy.MaxHp;
    public float MinHp => _enemy.MinHp;

    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action onHpMax;
    public event Action onHpMin;


    public void DepleteHp(object subject, float value)
    {
        _enemy.DepleteHp(subject, value * _multiple);
    }

    public void RecoverHp(object subject, float value)
    {

    }
}
