using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> �þ߿� ���õ� ����, ����� ��Ƶ� Ŭ���� </summary>
public class FieldOfView : MonoBehaviour
{
    [Header("Option")]
    [SerializeField] bool _debugMode = false;
    [SerializeField][Range(0f, 360f)] float _viewAngle = 0f;
    [SerializeField] float _viewRadius = 1f;
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    private List<Collider> _hitTargetList = new List<Collider>();
    private Collider[] _targets = new Collider[0];
    private Coroutine _fieldOfViewDetectionRoutine;


    private void OnEnable()
    {
        if (_fieldOfViewDetectionRoutine != null)
            StopCoroutine(_fieldOfViewDetectionRoutine);

        _fieldOfViewDetectionRoutine = StartCoroutine(FieldOfViewDetection());
    }


    private void OnDisable()
    {
        if (_fieldOfViewDetectionRoutine != null)
            StopCoroutine(_fieldOfViewDetectionRoutine);
    }


    /// <summary> �þ߰� �ȿ� ���� Ÿ�� ����Ʈ�� ù��°�� ��ȯ�ϴ� �Լ� </summary>
    public Transform GetTargetTransform()
    {
        if (0 < _hitTargetList.Count)
            return _hitTargetList[0].transform;

        return null;
    }


    /// <summary> �þ߰����� ������Ʈ��, ������ ���̾�鿡 ���ϴ� ������Ʈ�� ã�Ƴ� ����Ʈ�� �ִ� �Լ� </summary>
    private IEnumerator FieldOfViewDetection()
    {
        while (true)
        {
            yield return YieldCache.WaitForSeconds(1f);
            _hitTargetList.Clear();

            //OverlapSphere�� ���, ���� ������Ʈ�� ���� �������� ������ ���̾ ���� ������Ʈ�� Ž��
            Vector3 myPos = transform.position + Vector3.up;
            Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
            _targets = Physics.OverlapSphere(myPos, _viewRadius, _targetMask);

            if (_targets.Length == 0)
                continue;

            //Ž���� ������Ʈ�� �ݺ������� ������.
            foreach (Collider EnemyColli in _targets)
            {
                //������ ������Ʈ�� ��ġ�� �Ÿ�, ������ ����ϰ�, ������ �þ߰� �ȿ� ���� ���¶�� ����Ʈ�� �߰��Ѵ�.
                Vector3 targetPos = EnemyColli.transform.position + Vector3.up * 1.5f;
                float targetDistance = Vector3.Distance(myPos, targetPos);
                Vector3 targetDir = (targetPos - myPos).normalized;

                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

                if (targetAngle <= _viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, targetDistance, _obstacleMask))
                {
                    _hitTargetList.Add(EnemyColli);
                }
            }

        }
    }


    /// <summary> ������ ���Ⱚ���� ������ �����ϴ� �Լ� </summary>
    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }


    private void OnDrawGizmos()
    {
        if (!_debugMode) return;
        Vector3 myPos = transform.position + Vector3.up * 1.5f;
        Gizmos.DrawWireSphere(myPos, _viewRadius);

        float lookingAngle = transform.eulerAngles.y;  //ĳ���Ͱ� �ٶ󺸴� ������ ����
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + _viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - _viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * _viewRadius, Color.cyan);

        if (_targets.Length < 0)
            return;

        foreach (Collider EnemyColli in _targets)
        {
            Vector3 targetPos = EnemyColli.transform.position + Vector3.up * 1.5f;
            float targetDistance = Vector3.Distance(myPos, targetPos);
            Vector3 targetDir = (targetPos - myPos).normalized;
            Debug.DrawRay(myPos, targetDir * targetDistance, Color.red);
        }
    }
}
