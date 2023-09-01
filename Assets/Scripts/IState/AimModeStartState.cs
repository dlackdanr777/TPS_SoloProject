using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeStartState : IState
{
    private Player _player;
    public AimModeStartState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {
        _player.MyAnimator.SetBool("AimMode", true);
    }

    public void OnUpdate()
    {
        _player.WalkMovement();
        _player.PlayerRotate();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        if (_player.PlayerCamera.ZoomIn()) _player.ChangeState(_player.AimModeLoopState);
    }
}
