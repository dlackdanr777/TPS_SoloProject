using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimModeStartState : IUpperState
{
    private Player _player;
    private PlayerStateMachine _machine;
    public AimModeStartState(Player player, PlayerStateMachine machine)
    {
        _player = player;
        _machine = machine;
    }

    public void OnStart()
    {
        _player.Animator.SetBool("AimMode", true);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        _player.OnRotateHandler?.Invoke();
        _player.OnAimEnableHandler?.Invoke();
        _player.Rigging.SetUpperRigWeight(1.1f);
    }

    public void OnExit()
    {

    }

    public void OnStateUpdate()
    {
        if (_player.PlayerCamera.ZoomIn()) _machine.ChangeState(_machine.AimModeLoopState);
    }
}
