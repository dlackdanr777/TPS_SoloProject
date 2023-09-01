using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    private Player _player;

    public WalkState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {
 
    }

    public void OnUpdate()
    {
        _player.WalkMovement();
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
        _player.ChangeToIdleState();
        _player.ChangeToRunState();
        _player.ChangeToAimModeState();
    }
}
