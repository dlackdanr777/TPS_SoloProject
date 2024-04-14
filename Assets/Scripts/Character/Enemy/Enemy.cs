using System;
using System.Collections;
using UnityEngine;


/// <summary>몬스터와 관련된 컴포넌트, 정보를 관리하는 클래스</summary>
public class Enemy : MonoBehaviour, IHp
{
    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action<Vector3> OnRotatedHandler;
    public event Action onHpMax;
    public event Action onHpMin;
    public event Action OnTargetDamaged;
    public Action OnTargetLossHandler;
    public Action OnTargetFollowedHandler;


    [Header("Components")]
    public AudioSource AudioSource;
    public Navmesh Navmesh;
    public Animator Animator;
    public EnemySounds ZombieSounds;
    public FieldOfView FieldOfView;
    public EnemyAttack Attack;  
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private ParticleSystem _deadBloodParticle;

    [Space]
    [Header("Status")]
    [SerializeField] private ZombieType _zombieType;
    public ZombieType ZombieType => _zombieType;

    [SerializeField] private HpData _hpData;
    public float MaxHp => _hpData.MaxHp;

    public float MinHp => _hpData.MinHp;

    private float _hp;
    public float hp
    {
        get => _hp;
        private set
        {
            if (_hp == value)
                return;

            if (value > MaxHp)
                value = MaxHp;
            else if (value < MinHp)
                value = MinHp;

            _hp = value;
            onHpChanged?.Invoke(value);
            if (_hp == MaxHp)
                onHpMax?.Invoke();
            else if (_hp == MinHp)
                onHpMin?.Invoke();
        }
    }

    [HideInInspector] public Transform Target;
    private bool _isDead;
    public bool IsDead => _isDead;
    private EnemyStateMachine _machine;


    private void Awake()
    {
        _machine = new EnemyStateMachine(this);
    }


    private void Start()
    {
        Init();
        ActionInit();
    }


    private void Update()
    {
        if (_isDead && GameManager.Instance.IsGameEnd)
            return;

        _machine.OnUpdate();
    }


    private void FixedUpdate()
    {
        if (_isDead && GameManager.Instance.IsGameEnd)
            return;

        _machine.OnFixedUpdate();
    }

    private void OnEnable()
    {
        Init();
    }


    private void Init()
    {
        _hp = MaxHp;

        _isDead = false;
        _capsuleCollider.enabled = true;
        _machine.ChangeState(_machine.IdleState);
    }


    private void ActionInit()
    {
        onHpMin = () =>
        {
            if (!_isDead)
            {
                StartCoroutine(ObjectPoolManager.Instance.DisableZombie(this));
                StartCoroutine(StartDeadParticle());
                ZombieSounds.PlayZombieSoundClip(EnemySoundType.Dead);
                Navmesh.NaveMeshEnabled(false);
                _capsuleCollider.enabled = false;
                _machine.ChangeState(_machine.DeadState);

                Animator.SetTrigger("Dead");
                Target = null;
                _isDead = true;
            }
        };

        OnTargetFollowedHandler = () => { if (Target != null) Navmesh.StartNavMesh(Target); };
        OnTargetLossHandler = () => { if (Target == null) Navmesh.StopNavMesh(); };
    }


    /// <summary>사망 시 파티클을 실행하는 코루틴</summary>
    IEnumerator StartDeadParticle()
    {
        yield return YieldCache.WaitForSeconds(3);
        _deadBloodParticle.Emit(3);
    }


    public void RecoverHp(object subject, float value)
    {
        if (_isDead)
            return;

        hp += value;
        OnHpRecoverd?.Invoke(subject, value);
    }


    public void DepleteHp(object subject, float value)
    {
        if (_isDead)
            return;

        if (subject is Player)
        {
            Player player = (Player)subject;
            Target = player.transform;
        }

        hp -= value;
        OnHpDepleted?.Invoke(subject, value);
    }
}
