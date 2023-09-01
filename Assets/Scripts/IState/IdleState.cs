using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private Player _player;

    public IdleState(Player player)
    {
        _player = player;
    }

    public void OnStart()
    {

    }

    public void OnUpdate()
    {
        _player.Movement();
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }

}
