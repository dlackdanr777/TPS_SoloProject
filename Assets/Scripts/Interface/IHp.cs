using System;


/// <summary>체력 관련 인터페이스</summary>
public interface IHp 
{
    float hp { get; }
    float maxHp { get; }
    float minHp { get; }

    event Action<float> onHpChanged;
    event Action<object, float> OnHpRecoverd;
    event Action<object, float> OnHpDepleted;
    event Action onHpMax;
    event Action onHpMin;

    /// <summary>체력을 회복하는 함수</summary>
    void RecoverHp(object subject, float value);

    /// <summary>체력을 감소시키는 함수</summary>
    void DepleteHp(object subject, float value);
}
