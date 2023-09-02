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
        _player.MyAnimator.SetBool("IsRun", true);
    }

    public void OnUpdate()
    {
        _player.RunMovement(_machine.HorizontalInput, _machine.VerticalInput);
        _player.PlayerRotate();

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
        _machine.ChangeToIdleState();
        _machine.ChangeToWalkState();
    }
}
