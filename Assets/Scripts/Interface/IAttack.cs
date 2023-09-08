using System;

public interface IAttack
{
    float damage { get; }

    event Action<IHp, float> OnTargetDamaged;

    void TargetDamage(IHp iHp, float aomunt);
}
