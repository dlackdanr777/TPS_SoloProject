using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>���� ���� ���� ����, �Լ��� ������ �Լ�</summary>
public class EnemyAttack : MonoBehaviour, IAttack
{

    [Header("Option")]
    [SerializeField] private float _damage;
    public float Damage => _damage;
    [SerializeField] private float _attackRadius;
    public float attackRadius => _attackRadius;
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private LayerMask _obstacleMask;

    public event Action OnTargetDamaged;
    public event Action OnAttackHendler;
    private Coroutine CheckPlayerAtAttackRangeRoutine;
    private List<Collider> _hitColliderList = new List<Collider>();


    private void OnEnable()
    {
        //��ũ��Ʈ Ȱ��ȭ�� ���ݹ��� ���� ���� Ž���ϴ� �ڷ�ƾ ����
        if (CheckPlayerAtAttackRangeRoutine != null)
            StopCoroutine(CheckPlayerAtAttackRangeRoutine);

        CheckPlayerAtAttackRangeRoutine = StartCoroutine(CheckPlayerAtAttackRange(1f));
    }


    private void OnDisable()
    {
        if (CheckPlayerAtAttackRangeRoutine != null)
            StopCoroutine(CheckPlayerAtAttackRangeRoutine);
    }


    /// <summary>�ش� IHp�� ���� Ÿ�ٿ��� �������� �ִ� �Լ�</summary>
    public void TargetDamage(IHp iHp, float aomunt)
    {
        if (iHp.Hp < iHp.MinHp)
            return;

        iHp.DepleteHp(this, aomunt);
        OnTargetDamaged?.Invoke();
    }


    /// <summary>���� ������ �������� ��ȯ�ϴ� �Լ�</summary>
    public bool GetCheckPlayerAtAttackRange()
    {

        if (0 < _hitColliderList.Count)
            return true;

        return false;
    }


    /// <summary>���� ���� ����Ʈ�� ��ȸ�ϸ� IHp �������̽��� �����ϸ� DepleteHp()�� �����ϴ� �Լ�(�ִϸ��̼� �̺�Ʈ�� ����)</summary>
    public void StartAttack()
    {
        foreach (Collider collider in _hitColliderList)
        {
            if (!collider.TryGetComponent(out IHp iHp))
                continue;

            iHp.DepleteHp(this, _damage);
            OnAttackHendler?.Invoke();

            return;
        }
    }


    public void OnDrawGizmos()
    {
        Vector3 attackPos = transform.position + transform.up + (transform.forward * attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }


    /// <summary>���� ���� �� �ݶ��̴����� ����Ʈ�� �ִ� �Լ�</summary>
    private IEnumerator CheckPlayerAtAttackRange(float repeatCycle)
    {
        while (true)
        {
            yield return YieldCache.WaitForSeconds(repeatCycle);
            _hitColliderList.Clear();

            //���� �����ȿ� Ÿ���� �ִ��� Ȯ�� �� ���� ��� �ƹ��͵� ���� �ʴ´�.
            Vector3 attackPos = transform.position + transform.up + (transform.forward * attackRadius);
            Vector3 myPos = transform.position + Vector3.up;
            Collider[] Targets = Physics.OverlapSphere(attackPos, attackRadius, _attackLayerMask);
            if (Targets.Length <= 0)
                continue;

            //Ÿ���� ������� �ش� Ÿ�� ���� ��ȸ�ϸ� ���� ���� ����Ʈ�� �ִ´�. 
            foreach (Collider EnemyColli in Targets)
            {
                Vector3 targetPos = EnemyColli.transform.position + Vector3.up;
                Vector3 targetDir = (targetPos - myPos).normalized;
                float targetDistance = Vector3.Distance(targetPos, myPos);

                if (!Physics.Raycast(myPos, targetDir, targetDistance, _obstacleMask))
                {
                    _hitColliderList.Add(EnemyColli);
                }
            }
        }
    }
}
