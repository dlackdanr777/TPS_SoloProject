using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchWalkState : ILowerState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public CrouchWalkState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
        _player.WalkMovement(_machine.HorizontalInput * 0.5f, _machine.VerticalInput * 0.5f);
        _player.PlayerRotate();

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
    }
}
