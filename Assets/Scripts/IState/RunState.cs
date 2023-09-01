using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private Player _player;

    public RunState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {
        _player.MyAnimator.SetBool("IsRun", true);
    }

    public void OnUpdate()
    {
        _player.RunMovement();
        _player.PlayerRotate();

        _player.PlayerCamera.CameraCorrection();
    }

    public void OnFixedUpdate()
    {
    }

    public void OnExit()
    {
        _player.MyAnimator.SetBool("IsRun", false);
    }

    public void OnStateUpdate()
    {
        _player.ChangeToIdleState();
        _player.ChangeToWalkState();
        _player.ChangeToAimModeState();
    }
}
