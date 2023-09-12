using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHp, IAttack
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
    public float damage => _damage;
    public float attackRadius => _attackRadius;

    public event Action<float> onHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action<Vector3> OnRotatedHandler;
    public event Action onHpMax;
    public event Action onHpMin;
    public event Action OnTargetDamaged;

    public Action OnTargetFollowedHandler;

    public AudioSource AudioSource;
    public Navmesh Navmesh;
    public Animator Animator;
    public FieldOfView FieldOfView;
    public Transform Target;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    List<Collider> _hitColliders = new List<Collider>();

    [SerializeField] private float _maxHp;
    [SerializeField] private float _minHp = 0;
    private float _hp;

    [SerializeField] private float _damage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private LayerMask _obstacleMask;
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
    }

    private void ActionInit()
    {
        onHpMin = () =>
        {
            if (!_isDead)
            {
                StartCoroutine(ObjectPoolManager.Instance.ZombleDisable(this));
                Navmesh.NaveMeshEnabled(false);
                _capsuleCollider.enabled = false;
                _machine.ChangeState(_machine.IdleState);
                Animator.SetTrigger("Dead");
                _isDead = true;
            }
        };

        OnTargetFollowedHandler = () => { if (Target != null) Navmesh.StartNavMesh(Target); };
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

    public void TargetDamage(IHp iHp, float aomunt)
    {
        if (iHp.hp > iHp.minHp)
        {
            iHp.DepleteHp(this, aomunt);
            OnTargetDamaged?.Invoke();
        }
    }


    public bool CheckPlayerAtAttackRange()
    {
        _hitColliders.Clear();

        Vector3 attackPos = transform.position + transform.up + (transform.forward * attackRadius);
        Vector3 myPos = transform.position + Vector3.up;
        Collider[] Targets = Physics.OverlapSphere(attackPos, attackRadius, _attackLayerMask);
        if (Targets.Length != 0)
        {
            foreach (Collider EnemyColli in Targets)
            {
                Vector3 targetPos = EnemyColli.transform.position;
                Vector3 targetDir = (targetPos - myPos).normalized;
                float targetDistance = Vector3.Distance(targetPos, myPos);
                if (!Physics.Raycast(myPos, targetDir, targetDistance, _obstacleMask))
                {
                    _hitColliders.Add(EnemyColli);
                }
            }
        }

        if (_hitColliders.Count > 0)
            return true;
        return false;
    }

    public void Attack()
    {
        foreach(Collider collider in _hitColliders)
        {
            if(collider.GetComponent<IHp>() != null)
            {
                collider.GetComponent<IHp>().DepleteHp(this, _damage);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Vector3 attackPos = transform.position + transform.up + (transform.forward * attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }
}
