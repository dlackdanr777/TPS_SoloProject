using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrouchIdleState : ILowerState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public CrouchIdleState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
        _player.MyAnimator.SetBool("Crouch", true);
    }

    public void OnUpdate()
    {
        if (_machine.UpperCurrentState == _machine.AimModeLoopState)
            _player.PlayerCamera.ZoomIn();
    }

    public void OnFixedUpdate()
    {
    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {

        if ((_machine.HorizontalInput != 0 || _machine.VerticalInput != 0))
            _machine.ChangeState(_machine.CrouchWalkState);

        if (_machine.CrouchKeyPressed)
        {
            _machine.ChangeState(_machine.IdleState);
            _player.MyAnimator.SetBool("Crouch", false);
        }

    }
}
