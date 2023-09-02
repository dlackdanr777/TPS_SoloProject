using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : IState
{
    private Player _player;

    public CrouchState(Player player)
    {
        _player = player;
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
    }
}
