using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class AnimatedMesh : MonoBehaviour
{
    [SerializeField] private AnimatedMeshScriptableObject _animationSO;
    [SerializeField] private MeshCollider _meshCollider;
    private MeshFilter _filter;
     
    [Space]
    [SerializeField] private int _tick = 1;
    [SerializeField] private int _animationIndex;
    public int AnimationIndex => _animationIndex;
    [SerializeField] private string _animationName;

    private List<AnimatedMeshScriptableObject.MeshData> _meshDataList;
    private float _lastTickTime;

    public event Action<string> OnAnimationEnd;


    private void Awake()
    {
        _filter = GetComponent<MeshFilter>();
    }


    public void Play(string animationName)
    {
        if(_animationName != animationName)
        {
            _animationName = animationName;
            _tick = 1;
            _animationIndex = 0;
            AnimatedMeshScriptableObject.Animation animation = _animationSO.Animations.Find((item) => item.Name.Equals(animationName));
            _meshDataList = animation.MeshDataList;
            if (string.IsNullOrEmpty(animation.Name))
                Debug.LogError("�ִϸ��̼� �̸��� ����ֽ��ϴ�.");
        }
    }

    public List<Vector3> GetMeshVertexList()
    {
        if (_meshDataList == null)
            throw new Exception("���ؽ� ������ �����ϴ�.");

        return _meshDataList[_animationIndex].VertexList;
    }

    public Vector3 GetMeshVertex(int index)
    {
        if (_meshDataList == null)
            throw new Exception("���ؽ� ������ �����ϴ�.");

        if (_meshDataList[_animationIndex].VertexList.Count <= index || index < 0)
            throw new Exception("�ε��� ������ �۰ų� Ů�ϴ�.");

        return _meshDataList[_animationIndex].VertexList[index];
    }

    public Vector3 GetMeshNomal(int index)
    {
        if (_meshDataList == null)
            throw new Exception("���ؽ� ������ �����ϴ�.");

        if (_meshDataList[_animationIndex].VertexList.Count <= index || index < 0)
            throw new Exception("�ε��� ������ �۰ų� Ů�ϴ�.");

        return _meshDataList[_animationIndex].NomalList[index];
    }


    private void LateUpdate()
    {
        if (_meshDataList == null)
            return;

        if(Time.time >= _lastTickTime + (1f / _animationSO.AnimationFPS))
        {
            if (_meshCollider != null)
                _meshCollider.sharedMesh = _meshDataList[_animationIndex].Mesh;
            _filter.mesh = _meshDataList[_animationIndex].Mesh;

            _animationIndex++;
            if(_animationIndex >= _meshDataList.Count)
            {
                OnAnimationEnd?.Invoke(_animationName);
                _animationIndex = 0;
            }
            _lastTickTime = Time.time;
        }

        _tick++;
    }
}
