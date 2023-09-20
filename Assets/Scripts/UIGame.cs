using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIGame : MonoBehaviour
{
    [SerializeField] private UIStatus _hpBar;
    [SerializeField] private TextMeshProUGUI _bulletCountText;
    [SerializeField] private TextMeshProUGUI _gameTimerText;
    [SerializeField] private TextMeshProUGUI _zombieCountText;

    [SerializeField] private GameObject _uiWin;


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

        _uiWin.gameObject.SetActive(false);
    }

    public void SetGameTimerText(string text)
    {
        _gameTimerText.text = text;
    }

    public void SetZombieCountText(string text)
    {
        _zombieCountText.text = text;
    }

    public void ShowUIWin()
    {
        _uiWin.gameObject.SetActive(true);
    }

    private void ShowBulletCount()
    {
        _maxbulletCount = _player.GunController.CurrentGun.MaxBulletCount;
        _currentBulletCount = _player.GunController.CurrentGun.CurrentBulletCount;

        _bulletCountText.text = _currentBulletCount + " / " + _maxbulletCount;
    }




}
