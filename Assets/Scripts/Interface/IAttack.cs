using System;

public interface IAttack
{
    float damage { get; }

    event Action OnTargetDamaged;

    void TargetDamage(IHp iHp, float aomunt);
}
