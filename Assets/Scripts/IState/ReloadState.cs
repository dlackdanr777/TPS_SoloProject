using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : IUpperState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public ReloadState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
    }

    public void OnExit()
    {
    }

    public void OnStateUpdate()
    {
        if (!_machine.IsReload) _machine.ChangeState(_machine.BasicUpperState);
    }
}
