using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : IState
{
    private Player _player;

    public MovementState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
        _player.Movement();
        _player.PlayerRotate();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}
