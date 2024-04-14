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
        _player.OnFireHandler?.Invoke();
        _player.OnEnableAimHandler?.Invoke();
    }

    public void OnFixedUpdate()
    {
        _player.OnRotateHandler?.Invoke();

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        if (!_machine.AimModeEnable || _player.GunController.IsReload) _machine.ChangeState(_machine.AimModeEndState);
    }
}
