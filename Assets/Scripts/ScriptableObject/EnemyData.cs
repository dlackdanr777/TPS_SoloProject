using UnityEngine;

/// <summary>���� ���� ������ ������ Ŭ����</summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/Enemydata", order = 1)]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyType _enemyType;
    public EnemyType EnemyType => _enemyType;

    [SerializeField] private float _maxHp;
    public float MaxHp => _maxHp;

    [SerializeField] private float _minHp;
    public float MinHp => _maxHp;
}
