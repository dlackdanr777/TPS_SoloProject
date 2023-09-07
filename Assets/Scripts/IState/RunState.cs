using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunState : ILowerState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public RunState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
        _player.Animator.SetBool("IsRun", true);
    }

    public void OnUpdate()
    {
        _player.OnMovedHandler?.Invoke(_machine.HorizontalInput, _machine.VerticalInput, _player.PlayerMovement.RunSpeedMul);
        _player.OnRotateHandler?.Invoke();

    }

    public void OnFixedUpdate()
    {
    }

    public void OnExit()
    {
        _player.Animator.SetBool("IsRun", false);
    }

    public void OnStateUpdate()
    {
        _machine.ChangeToIdleState();
        _machine.ChangeToWalkState();
        _machine.ChangeToCrouchState();
    }
}
