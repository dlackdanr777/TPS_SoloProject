using System;
using UnityEngine;
public class HitPart : MonoBehaviour, IHp
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private string _partName;
    [SerializeField] private float _multiple;

    public float hp => throw new NotImplementedException();

    public float maxHp => throw new NotImplementedException();

    public float minHp => throw new NotImplementedException();

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
