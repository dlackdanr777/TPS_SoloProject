using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HpData", menuName = "Scriptable Object/Hpdata", order = 1)]
public class HpData : ScriptableObject
{
    [SerializeField] private float _maxHp;
    public float MaxHp => _maxHp;

    [SerializeField] private float _minHp;
    public float MinHp => _maxHp;
}
