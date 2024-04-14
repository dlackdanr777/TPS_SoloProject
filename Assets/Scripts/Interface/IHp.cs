using System;


/// <summary>ü�� ���� �������̽�</summary>
public interface IHp 
{
    float Hp { get; }
    float MaxHp { get; }
    float MinHp { get; }

    event Action<float> OnHpChanged;
    event Action<object, float> OnHpRecoverd;
    event Action<object, float> OnHpDepleted;
    event Action OnHpMax;
    event Action OnHpMin;

    /// <summary>ü���� ȸ���ϴ� �Լ�</summary>
    void RecoverHp(object subject, float value);

    /// <summary>ü���� ���ҽ�Ű�� �Լ�</summary>
    void DepleteHp(object subject, float value);
}
