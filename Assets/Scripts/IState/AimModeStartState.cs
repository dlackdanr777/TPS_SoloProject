using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimModeStartState : IUpperState
{
    private Player _player;
    private PlayerStateMachine _machine;
    public AimModeStartState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
        _player.MyAnimator.SetBool("AimMode", true);
    }

    public void OnUpdate()
    {
        _player.Movement(_machine.HorizontalInput, _machine.VerticalInput);
        _player.PlayerRotate();
        _player.PlayerCamera.CrossHairEnable();
        _player.SetRiggingWeight(1.1f);
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        if (_player.PlayerCamera.ZoomIn()) _machine.ChangeState(_machine.AimModeLoopState);
    }
}
