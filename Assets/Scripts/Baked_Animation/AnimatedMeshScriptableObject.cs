using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Bake�� �ִϸ��̼� �޽��� �����ϴ� ��ũ���ͺ� ������Ʈ</summary>
public class AnimatedMeshScriptableObject : ScriptableObject
{
    public int AnimationFPS;
    public List<Animation> Animations = new List<Animation>();

    [Serializable]
    public struct Animation
    {
        public string Name;
        public List<Mesh> Meshes;
    }

}
