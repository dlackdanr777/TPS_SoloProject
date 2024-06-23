using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�ǰ� ����</summary>
public class HitPart : MonoBehaviour, IHp
{
    [Header("Components")]
    [SerializeField] private Enemy _enemy;
    [SerializeField] private AnimatedMesh _animatedMesh;

    [Space]
    [Header("Options")]
    [SerializeField] private string _partName;
    [SerializeField] private float _multiple;

    public float Hp => _enemy.Hp;
    public float MaxHp => _enemy.MaxHp;
    public float MinHp => _enemy.MinHp;

    public event Action<float> OnHpChanged;
    public event Action<object, float> OnHpRecoverd;
    public event Action<object, float> OnHpDepleted;
    public event Action OnHpMax;
    public event Action OnHpMin;

    private MeshFilter _meshFilter;
    private Vector3 _correctionPos;
    private int _initialVertexIndex = -1;


    public void DepleteHp(object subject, float value)
    {
        _enemy.DepleteHp(subject, value * _multiple);
    }

    public void RecoverHp(object subject, float value)
    {

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_initialVertexIndex == -1)
            return;

        // �޽� ���� ��ġ ����
        Vector3 worldPosition = _meshFilter.transform.TransformPoint(_animatedMesh.GetMeshVertex(_initialVertexIndex));
        transform.position = _correctionPos + worldPosition;

        // ������ ��� ���� ����Ͽ� ȸ�� �� ���
        Vector3 normal = _meshFilter.transform.TransformDirection(_animatedMesh.GetMeshNomal(_initialVertexIndex));

        Quaternion rotation = Quaternion.LookRotation(normal);
        transform.rotation = rotation;
    }

    private void Init()
    {
        _meshFilter = _animatedMesh.GetComponent<MeshFilter>();
        // �޽��� ���� �� �ϳ��� �ʱ� ��ġ�� ���� (���� ����� ���� ã��)
        _initialVertexIndex = FindClosestVertexIndex(transform.position, _meshFilter, _animatedMesh);
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

        _correctionPos =  meshFilter.transform.TransformPoint(vertices[closestVertexIndex]) - transform.position;
        return closestVertexIndex;
    }


}
