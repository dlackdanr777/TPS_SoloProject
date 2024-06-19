using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletHole : MonoBehaviour
{
    [SerializeField] private AudioSource _AudioSource;
    [SerializeField] private AudioClip _hitPartClip;
    [SerializeField] private AudioClip _groundClip;
    [SerializeField] private float _disableTime;

    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private ParticleSystem _bloodCloudParticle;

    private MeshFilter _meshFilter;
    private AnimatedMesh _animatedMesh;
    private int _initialVertexIndex;
    private bool _isAttachedToMesh = false;


    private void OnEnable()
    {
        StartCoroutine(BulletHoleDisable(_disableTime));
        StartCoroutine(TargetHit());
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
        _isAttachedToMesh = false;
    }

    private IEnumerator BulletHoleDisable(float time)
    {
        yield return YieldCache.WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private IEnumerator TargetHit()
    {
        yield return YieldCache.WaitForSeconds(0.01f);

        if (transform.parent.CompareTag("HitPart") || transform.parent.CompareTag("Enemy"))
        {
            if (transform.parent.TryGetComponent(out _meshFilter))
            {
                _animatedMesh = transform.parent.GetComponent<AnimatedMesh>();
                _isAttachedToMesh = true;
                // �޽��� ���� �� �ϳ��� �ʱ� ��ġ�� ���� (���� ����� ���� ã��)
                _initialVertexIndex = FindClosestVertexIndex(transform.position, _meshFilter, _animatedMesh);
            }

            _AudioSource.PlayOneShot(_hitPartClip);
            PlayParticle(2);
            yield return YieldCache.WaitForSeconds(0.2f);
            PlayParticle(1);
        }
        else if (transform.parent.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _AudioSource.PlayOneShot(_groundClip);
        }
    }

    private int FindClosestVertexIndex(Vector3 position, MeshFilter meshFilter, AnimatedMesh animatedMesh)
    {
        List<Vector3> vertices = animatedMesh.GetMeshVertexList();

        int closestVertexIndex = 0;
        float minDistance = Vector3.Distance(position, meshFilter.transform.TransformPoint(vertices[0]));

        for (int i = 1; i < vertices.Count; i++)
        {
            float distance = Vector3.Distance(position, meshFilter.transform.TransformPoint(vertices[i]));
            if (distance < minDistance)
            {
                closestVertexIndex = i;
                minDistance = distance;
            }
        }

        return closestVertexIndex;
    }

    private void Update()
    {
        if (_isAttachedToMesh && _meshFilter != null)
        {
            // �޽� ���� ��ġ ����
            Vector3 worldPosition = _meshFilter.transform.TransformPoint(_animatedMesh.GetMeshVertex(_initialVertexIndex));
            transform.position = worldPosition;

            // ������ ��� ���� ����Ͽ� ȸ�� �� ���
            Vector3 normal = _meshFilter.transform.TransformDirection(_animatedMesh.GetMeshNomal(_initialVertexIndex));

            Quaternion rotation = Quaternion.LookRotation(normal);
            transform.rotation = rotation;
        }
    }

    private void PlayParticle(int amount)
    {
        _bloodParticle.Emit(amount);
        _bloodCloudParticle.Emit(amount);
    }
}
