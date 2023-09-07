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
        _player.OnRotateHandler?.Invoke();
        _player.OnAimEnableHandler?.Invoke();

        _player.OnFireHandler?.Invoke();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        if (!_machine.AimModeEnable || _player.GunController.IsReload) _machine.ChangeState(_machine.AimModeEndState);
    }
}
