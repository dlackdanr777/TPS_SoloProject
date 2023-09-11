using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField] private UIStatus _hpBar;

    public void Start()
    {
        GameManager.Instance.Player.onHpChanged += _hpBar.AmountChanged;
        _hpBar.Init(GameManager.Instance.Player.maxHp);
    }
}
