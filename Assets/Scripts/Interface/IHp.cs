using System;

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

    void HpRecoverHp(object subject, float value);
    void HpDepleteHp(object subject, float value);
}
