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
 
    }

    public void OnUpdate()
    {
        _player.WalkMovement(_machine.HorizontalInput, _machine.VerticalInput);
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
    }
}
