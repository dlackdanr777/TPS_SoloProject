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
        _player.OnSetRecoilSizeHandler?.Invoke(1f);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        _player.OnMovedHandler?.Invoke(_machine.HorizontalInput, _machine.VerticalInput, 0.5f);

        _player.OnRotateHandler?.Invoke();

        if (_machine.UpperCurrentState == _machine.AimModeLoopState)
            _player.PlayerCamera.ZoomIn();
    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {

        if (_machine.HorizontalInput == 0 && _machine.VerticalInput == 0)
            _machine.ChangeState(_machine.CrouchIdleState);

        if (_machine.CrouchKeyPressed)
        {
            _machine.ChangeState(_machine.IdleState);
            _player.Animator.SetBool("Crouch", false);
        }
    }
}
