using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpperState : IUpperState
{
    private Player _player;
    private PlayerStateMachine _machine;
    public BasicUpperState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
    }

    public void OnUpdate()
    {
        _player.PlayerCamera.CameraCorrection();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        _machine.ChangeToAimModeState();
    }
}
