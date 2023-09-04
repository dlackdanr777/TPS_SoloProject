using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : ILowerState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public WalkState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
        _player.GunController.SetRecoilMul(2f);
    }

    public void OnUpdate()
    {
        if(_machine.UpperCurrentState == _machine.AimModeLoopState)
            _player.Movement(_machine.HorizontalInput, _machine.VerticalInput, 0.5f);
        else
            _player.Movement(_machine.HorizontalInput, _machine.VerticalInput);

        if (_machine.UpperCurrentState == _machine.AimModeLoopState)
            _player.PlayerCamera.ZoomIn();

        _player.PlayerRotate();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        _machine.ChangeToIdleState();
        _machine.ChangeToRunState();
        _machine.ChangeToCrouchState();
    }
}
