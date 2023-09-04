using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeEndState : IUpperState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public AimModeEndState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
        _player.MyAnimator.SetBool("AimMode", false);
        _player.GunController.CrossHairDisable();
    }

    public void OnUpdate()
    {
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
        if (_player.PlayerCamera.ZoomOut())
        {
            if (_machine.IsReload) _machine.ChangeState(_machine.ReloadState);
            else  _machine.ChangeState(_machine.BasicUpperState); 
        }         
    }
}
