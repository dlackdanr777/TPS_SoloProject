using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IHp, IAttack
{
    [Header("컴포넌트")]
    public Camera MainCamera;
    public CinemachineCamera PlayerCamera;
    public Animator Animator;
    public GunController GunController;
    public Inventory Inventory;
    public Rigging Rigging;
    public PlayerMovement PlayerMovement;
    public FlashLight FlashLight;
    public PlayerStateMachine Machine;
    public CharacterController CharacterController;

    public Dictionary<int, string> a;
    public event Action onHpMax;
    public event Action onHpMin;
    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnTargetDamaged;
    public Action<float, float, float> OnMovedHandler;
    public Action<float> OnSetRecoilSizeHandler;
    public Action OnRotateHandler;
    public Action OnEnableAimHandler;
    public Action OnDisableAimHandler;
    public Action OnFollowAimHandler;
    public Action OnFireHandler;


    public float hp
    {
         get => _hp;
        private set
        {
            if (_hp == value)
                return;

            if(value > _maxHp)
                value = _maxHp;
            else if(value < _minHp)
                value = _minHp;

            _hp = value;
            onHpChanged?.Invoke(value);
            if (_hp == _maxHp)
                onHpMax?.Invoke();
            else if(_hp == _minHp)
                onHpMin?.Invoke();
        }
    }
    public float maxHp => _maxHp;
    public float minHp => _minHp;
    public float damage { get; }

    private float _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;

    private void Awake()
    {
        Machine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        GameManager.Instance.Player = this;
        Init();
        ActionInit();
        OnDisableAimHandler?.Invoke();
    }


    private void Update()
    {
        if (!GameManager.Instance.IsGameEnd)
        {
            Machine.OnUpdate();
            OnFollowAimHandler?.Invoke();
            FlashLight.ControllFlash();
        }

    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameEnd)
        {
            Machine.OnFixedUpdate();
        }    
    }

    private void Init()
    {
        _hp = _maxHp;
    }

    private void ActionInit()
    {
        OnRotateHandler = PlayerMovement.Rotate;      
        
        OnMovedHandler = PlayerMovement.Movement;
        OnMovedHandler += (horizontal, vertical, speedMul) =>
        {
            Animator.SetFloat("Vertical", vertical);
            Animator.SetFloat("Horizontal", horizontal);
        };

        OnEnableAimHandler = GunController.EnableCrossHair;
        OnDisableAimHandler = GunController.DisableCrossHair;
        OnFollowAimHandler = GunController.FollowCrossHair;

        OnFireHandler = GunController.TryFire;
        OnSetRecoilSizeHandler = GunController.SetRecoilMul;

        GunController.OnTargetDamageHendler += TargetDamage;
        GunController.OnFireHendler += PlayerCamera.CameraShakeStart;

        onHpMin += () =>
        {
            if (!GameManager.Instance.IsGameEnd)
            {
                Animator.SetBool("IsDead", true);
                GameManager.Instance.IsGameEnd = true;
                GameManager.Instance.CursorVisible();
                Machine.ChangeState(Machine.DeadState);
                CharacterController.enabled = false;
            }
        };
    }

    public void RecoverHp(object subject, float value)
    {
        hp += value;
        OnHpRecoverd?.Invoke(subject, value);
    }

    public void DepleteHp(object subject, float value)
    {
        hp -= value;
        OnHpDepleted?.Invoke(subject, value);
    }

    public void TargetDamage(IHp ihp, float aomunt)
    {
        if(ihp.hp > ihp.minHp)
        {
            ihp.DepleteHp(this, aomunt);
            OnTargetDamaged?.Invoke();
        }    
    }
}

