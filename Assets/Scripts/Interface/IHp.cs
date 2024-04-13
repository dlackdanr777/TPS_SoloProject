using System;


/// <summary>ü�� ���� �������̽�</summary>
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

    /// <summary>ü���� ȸ���ϴ� �Լ�</summary>
    void RecoverHp(object subject, float value);

    /// <summary>ü���� ���ҽ�Ű�� �Լ�</summary>
    void DepleteHp(object subject, float value);
}
