using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeLoopState : IState
{
    private Player _player;

    public AimModeLoopState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
        _player.WalkMovement();
        _player.PlayerRotate();
        _player.CrossHairEnable();

        _player.GunController.TryFire();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        if (!_player.AimModeEnable) _player.ChangeState(_player.AimModeEndState);
    }
}
