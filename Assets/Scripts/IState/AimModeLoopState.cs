using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeLoopState : IUpperState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public AimModeLoopState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
        _player.PlayerRotate();
        _player.GunController.CrossHairEnable();

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
        if (!_machine.AimModeEnable || _machine.IsReload) _machine.ChangeState(_machine.AimModeEndState);
    }
}
