
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] bool _debugMode = false;
    [Range(0f, 360f)][SerializeField] float _viewAngle = 0f;
    [SerializeField] float _viewRadius = 1f;
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    private List<Collider> _hitTargetList = new List<Collider>();

    public Transform GetTargetTransform()
    {
        if(_hitTargetList.Count > 0)
            return _hitTargetList[0].transform;
        return null;
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    private void OnDrawGizmos()
    {
        if (!_debugMode) return;
        Vector3 myPos = transform.position + Vector3.up;
        Gizmos.DrawWireSphere(myPos, _viewRadius);

        float lookingAngle = transform.eulerAngles.y;  //ĳ���Ͱ� �ٶ󺸴� ������ ����
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + _viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - _viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * _viewRadius, Color.cyan);

        _hitTargetList.Clear();
        Collider[] Targets = Physics.OverlapSphere(myPos, _viewRadius, _targetMask);

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= _viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, _viewRadius, _obstacleMask))
            {
                _hitTargetList.Add(EnemyColli);
                if (_debugMode) Debug.DrawLine(myPos, targetPos, Color.red);
            }
        }
    }
}
