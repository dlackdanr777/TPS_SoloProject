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
        _player.Animator.SetBool("Crouch", true);
        _player.OnSetRecoilSizeHandler?.Invoke(0.5f);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        if (_machine.UpperCurrentState == _machine.AimModeLoopState)
            _player.PlayerCamera.ZoomIn();
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
            _player.Animator.SetBool("Crouch", false);
        }

    }
}
