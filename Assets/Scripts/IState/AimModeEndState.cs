using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeEndState : IState
{
    private Player _player;

    public AimModeEndState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {
        _player.MyAnimator.SetBool("AimMode", false);

        _player.CrossHairDisable();
    }

    public void OnUpdate()
    {
        _player.WalkMovement();
        _player.PlayerRotate();
        _player.SetRiggingWeight(-0.1f);
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {
    }

    public void OnStateUpdate()
    {
        if (_player.PlayerCamera.ZoomOut()) _player.ChangeState(_player.IdleState);
        
    }
}
