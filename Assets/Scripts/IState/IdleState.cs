using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ILowerState
{
    private Player _player;
    private PlayerStateMachine _machine;

    public IdleState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        _machine.ChangeToWalkState();
        _machine.ChangeToRunState();
        _machine.ChangeToCrouchState();
    }

}
