using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IHp
{
    public ZombieType ZombieType;
    public float hp
    {
        get => _hp;
        private set
        {
            if (_hp == value)
                return;

            if (value > _maxHp)
                value = _maxHp;
            else if (value < _minHp)
                value = _minHp;

            _hp = value;
            onHpChanged?.Invoke(value);
            if (_hp == _maxHp)
                onHpMax?.Invoke();
            else if (_hp == _minHp)
                onHpMin?.Invoke();
        }
    }
    public float maxHp => _maxHp;
    public float minHp => _minHp;

    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action<Vector3> OnRotatedHandler;
    public event Action onHpMax;
    public event Action onHpMin;
    public event Action OnTargetDamaged;

    public Action OnTargetLossHandler;
    public Action OnTargetFollowedHandler;

    public AudioSource AudioSource;
    public Navmesh Navmesh;
    public Animator Animator;
    public ZombieSounds ZombieSounds;
    public FieldOfView FieldOfView;
    public Attack Attack;
    
    public Transform Target;
    
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private ParticleSystem _deadBloodParticle;

    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;
    private float _hp;

    private bool _isDead;

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
        if (!_isDead)
        {
            _machine.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!_isDead)
        {
            _machine.OnFixedUpdate();
        }
    }

    private void OnEnable()
    {
        Init();
    }


    private void Init()
    {
        _hp = _maxHp;
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
                StartCoroutine(ObjectPoolManager.Instance.ZombieDisable(this));
                StartCoroutine(StartDeadParticle());
                ZombieSounds.PlayZombieSoundClip(ZombieSounds.ZombieSoundType.Dead);
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

    IEnumerator StartDeadParticle()
    {
        yield return YieldCache.WaitForSeconds(3);
        _deadBloodParticle.Emit(3);
    }


    public void RecoverHp(object subject, float value)
    {
        if (!_isDead)
        {
            hp += value;
            OnHpRecoverd?.Invoke(subject, value);
        }
    }

    public void DepleteHp(object subject, float value)
    {
        if (!_isDead)
        {
            hp -= value;
            Debug.Log("피격되었습니다! 남은체력:" + _hp);
            if(subject is Player)
            {
                Player player = (Player)subject;
                Target = player.transform;
            }
            OnHpDepleted?.Invoke(subject, value);
        }
    }

}
