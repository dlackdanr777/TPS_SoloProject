using System;

/// <summary>���� ���� �������̽�</summary>
public interface IAttack
{
    float Damage { get; }

    event Action OnTargetDamaged;

    /// <summary>IHp�������̽��� DepleteHp�� ������� ü���� ���ҽ�Ű�� �Լ�</summary>
    void TargetDamage(IHp iHp, float aomunt);
}
