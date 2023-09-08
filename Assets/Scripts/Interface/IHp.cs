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

    void RecoverHp(object subject, float value);
    void DepleteHp(object subject, float value);
}
