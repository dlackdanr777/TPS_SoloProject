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
        _player.Animator.SetBool("AimMode", false);
        _player.OnDisableAimHandler?.Invoke();
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        _player.OnRotateHandler?.Invoke();
        _player.Rigging.SetUpperRigWeight(-0.1f);
    }

    public void OnExit()
    {
    }

    public void OnStateUpdate()
    {
        if (_player.PlayerCamera.ZoomOut())
        {
            if (_player.GunController.IsReload) _machine.ChangeState(_machine.ReloadState);
            else  _machine.ChangeState(_machine.BasicUpperState); 
        }         
    }
}
