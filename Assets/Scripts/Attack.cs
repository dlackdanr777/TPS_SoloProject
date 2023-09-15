using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{

    public float damage => _damage;
    public float attackRadius => _attackRadius;
    public event Action OnTargetDamaged;
    public event Action OnAttackHendler;


    List<Collider> _hitColliders = new List<Collider>();
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private LayerMask _obstacleMask;

    private Coroutine CheckPlayerAtAttackRangeRoutine;


    private void OnEnable()
    {
        if (CheckPlayerAtAttackRangeRoutine != null)
            StopCoroutine(CheckPlayerAtAttackRangeRoutine);
        CheckPlayerAtAttackRangeRoutine = StartCoroutine(CheckPlayerAtAttackRange(1f));
    }

    private void OnDisable()
    {
        if (CheckPlayerAtAttackRangeRoutine != null)
            StopCoroutine(CheckPlayerAtAttackRangeRoutine);
    }

    public void TargetDamage(IHp iHp, float aomunt)
    {
        if (iHp.hp > iHp.minHp)
        {
            iHp.DepleteHp(this, aomunt);
            OnTargetDamaged?.Invoke();
        }
    }
 
    public bool GetCheckPlayerAtAttackRange()
    {

        if (_hitColliders.Count > 0)
            return true;

        return false;
    }

    private IEnumerator CheckPlayerAtAttackRange(float repeatCycle)
    {
        while (true)
        {
            yield return YieldCache.WaitForSeconds(repeatCycle);
            _hitColliders.Clear();

            Vector3 attackPos = transform.position + transform.up + (transform.forward * attackRadius);
            Vector3 myPos = transform.position + Vector3.up;
            Collider[] Targets = Physics.OverlapSphere(attackPos, attackRadius, _attackLayerMask);
            if (Targets.Length != 0)
            {
                foreach (Collider EnemyColli in Targets)
                {
                    Vector3 targetPos = EnemyColli.transform.position + Vector3.up;
                    Vector3 targetDir = (targetPos - myPos).normalized;
                    float targetDistance = Vector3.Distance(targetPos, myPos);
                    if (!Physics.Raycast(myPos, targetDir, targetDistance, _obstacleMask))
                    {
                        _hitColliders.Add(EnemyColli);
                    }
                }
            }
        }
      


    }

    public void StartAttack()
    {
        foreach (Collider collider in _hitColliders)
        {
            if (collider.GetComponent<IHp>() != null)
            {
                collider.GetComponent<IHp>().DepleteHp(this, _damage);
                OnAttackHendler?.Invoke();
                
                return;
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
