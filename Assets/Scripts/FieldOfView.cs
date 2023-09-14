using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] bool _debugMode = false;
    [Range(0f, 360f)][SerializeField] float _viewAngle = 0f;
    [SerializeField] float _viewRadius = 1f;
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    private List<Collider> _hitTargetList = new List<Collider>();
    private Coroutine FieldOfViewDetectionRoutine;

    private void OnEnable()
    {
        if (FieldOfViewDetectionRoutine != null)
            StopCoroutine(FieldOfViewDetectionRoutine);
        FieldOfViewDetectionRoutine = StartCoroutine(FieldOfViewDetection());
    }

    private void OnDisable()
    {
        if (FieldOfViewDetectionRoutine != null)
            StopCoroutine(FieldOfViewDetectionRoutine);
    }

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

    IEnumerator FieldOfViewDetection()
    {
        while (true)
        {
            _hitTargetList.Clear();
            Vector3 myPos = transform.position + Vector3.up;
            Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
            Collider[] Targets = Physics.OverlapSphere(myPos, _viewRadius, _targetMask);

            if (Targets.Length != 0)
            {
                foreach (Collider EnemyColli in Targets)
                {
                    Vector3 targetPos = EnemyColli.transform.position + Vector3.up * 1.5f;
                    Vector3 targetDir = (targetPos - myPos).normalized;
                    float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                    if (targetAngle <= _viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, _viewRadius, _obstacleMask))
                    {
                        _hitTargetList.Add(EnemyColli);
                    }
                }
            }
            yield return YieldCache.WaitForSeconds(1f);
        }    
    }

    private void OnDrawGizmos()
    {
        if (!_debugMode) return;
        Vector3 myPos = transform.position + Vector3.up * 1.5f;
        Gizmos.DrawWireSphere(myPos, _viewRadius); 

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + _viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - _viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * _viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * _viewRadius, Color.cyan);


    }
}
