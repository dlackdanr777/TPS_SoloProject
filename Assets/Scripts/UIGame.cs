using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIGame : MonoBehaviour
{
    [SerializeField] private UIStatus _hpBar;
    [SerializeField] private TextMeshProUGUI _bulletCountText;


    private Player _player;

    private int _maxbulletCount;
    private int _currentBulletCount;
    public void Start()
    {
        _player = GameManager.Instance.Player;
        _player.onHpChanged += _hpBar.AmountChanged;
        _hpBar.Init(_player.maxHp);

        _player.GunController.OnFireHendler += ShowBulletCount;
        _player.GunController.OnReloadHendler += ShowBulletCount;
        ShowBulletCount();
    }

    private void ShowBulletCount()
    {
        _maxbulletCount = _player.GunController.CurrentGun.MaxBulletCount;
        _currentBulletCount = _player.GunController.CurrentGun.CurrentBulletCount;

        _bulletCountText.text = _currentBulletCount + " / " + _maxbulletCount;
    }
}
