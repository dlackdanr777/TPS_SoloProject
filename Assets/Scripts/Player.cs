using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    public Action<float, float, float> OnMovedHandler;
    public Action OnRotateHandler;
    public Action OnGravityHandler;
    public Action OnAimEnableHandler;
    public Action OnAimDisableHandler;
    public Action<float> OnSetRecoilSizeHandler;
    public Action OnFireHandler;

    [Header("컴포넌트")]
    public Camera MainCamera;
    public CinemachineCamera PlayerCamera;
    public Animator Animator;
    public GunController GunController;
    public Inventory Inventory;
    public Rigging Rigging;

    public PlayerMovement PlayerMovement;
    public PlayerStateMachine Machine;

    private void Awake()
    {
        Machine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        ActionInit();
    }


    private void Update()
    {
        OnGravityHandler();
        Machine.OnUpdate();
    }

    private void FixedUpdate()
    {
        Machine.OnFixedUpdate();
    }

    private void ActionInit()
    {
        OnGravityHandler = PlayerMovement.GravityEnable;
        OnRotateHandler = PlayerMovement.Rotate;

        OnMovedHandler = PlayerMovement.Movement;
        OnMovedHandler += (horizontal, vertical, speedMul) =>
        {
            Animator.SetFloat("Vertical", vertical);
            Animator.SetFloat("Horizontal", horizontal);
        };

        OnAimEnableHandler = GunController.CrossHairEnable;
        OnAimDisableHandler = GunController.CrossHairDisable;

        OnFireHandler = GunController.TryFire;
        OnSetRecoilSizeHandler = GunController.SetRecoilMul;
    }
}
