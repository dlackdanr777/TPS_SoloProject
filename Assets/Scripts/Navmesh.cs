using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navmesh : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _target;

    private Coroutine _coroutine;
    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        NaveMeshEnabled(true);
        _coroutine = StartCoroutine(UpdateDestination());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }


    public void TargetOn(Transform transform)
    {
        _target = transform;
        _agent.SetDestination(_target.position);
    }
    
    public void NaveMeshEnabled(bool value)
    {
        _target = null;
        _agent.enabled = value;
    }

    public void StopNavMesh()
    {
        _agent.isStopped = true;
        _target = null;
    }

    public void StartNavMesh(Transform transform)
    {
        _target = transform;
        _agent.SetDestination(_target.position);
        _agent.isStopped = false;
    }

    IEnumerator UpdateDestination()
    {
        while (true)
        {
            if(_target != null && _agent.enabled)
                _agent.SetDestination(_target.position);

            yield return YieldCache.WaitForSeconds(0.2f);
        }
    }

}
