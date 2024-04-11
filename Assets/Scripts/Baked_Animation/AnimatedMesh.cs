using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class AnimatedMesh : MonoBehaviour
{
    [SerializeField] private AnimatedMeshScriptableObject _animationSO;
    private MeshFilter _filter;
     
    [Space]
    [SerializeField] private int _tick = 1;
    [SerializeField] private int _animationIndex;
    [SerializeField] private string _animationName;

    private List<Mesh> _animationMeshes;
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
            _animationMeshes = animation.Meshes;
            if (string.IsNullOrEmpty(animation.Name))
                Debug.LogError("애니메이션 이름이 비어있습니다.");
        }
    }


    private void LateUpdate()
    {
        if (_animationMeshes == null)
            return;

        if(Time.time >= _lastTickTime + (1f / _animationSO.AnimationFPS))
        {
            _filter.mesh = _animationMeshes[_animationIndex];

            _animationIndex++;
            if(_animationIndex >= _animationMeshes.Count)
            {
                OnAnimationEnd?.Invoke(_animationName);
                _animationIndex = 0;
            }
            _lastTickTime = Time.time;
        }

        _tick++;
    }
}
