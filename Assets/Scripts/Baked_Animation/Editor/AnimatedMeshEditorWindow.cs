using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor; //����Ƽ ������ �󿡼� �ڷ�ƾ�� �� �� �ְ� ���ִ� namespace
using UnityEditor;
using UnityEngine;


/// <summary>�ִϸ��̼��� ���� �� Ŭ����</summary>
public class AnimatedMeshEditorWindow : EditorWindow
{
    private const string BASE_PATH = "Assets/Animated Models/";

    [MenuItem("Tools/Animated Mesh Creator")]
    //������ â�� �� �� �ְ� �ϴ� �Լ�
    public static void CreateEditorWindow()
    {
        EditorWindow window = GetWindow<AnimatedMeshEditorWindow>();
        window.titleContent = new GUIContent("Animated Mesh Editor");
    }

    private GameObject _animatedModel;
    private int _animationFPS = 30;
    private string _name;
    private bool _optimize = false;
    private bool _dryRun = false;

    private void OnGUI()
    {
        GameObject newModel = EditorGUILayout.ObjectField("Animated Model", _animatedModel, typeof(GameObject), true) as GameObject;
        if (newModel != _animatedModel)
            _name = newModel.name + "animations";

        Animator animator = newModel == null ? null : newModel.GetComponentInChildren<Animator>();
        _animatedModel = newModel;

        _name = EditorGUILayout.TextField("Name", _name);
        _animationFPS = EditorGUILayout.IntSlider("Animation FPS", _animationFPS, 1, 100);
        _optimize = EditorGUILayout.Toggle("Optimize", _optimize);
        _dryRun = EditorGUILayout.Toggle("Dry Run", _dryRun);

        GUI.enabled = newModel != null && animator.runtimeAnimatorController != null;
        if (GUILayout.Button("Generate ScriptableObjects"))
        {
            if (newModel == null)
                return;

            //���� ��θ� �����Ѵ�.
            AssetDatabase.GenerateUniqueAssetPath(BASE_PATH + _name);

            //�ִϸ������� Ŭ������ ���� ��ũ���ͺ� ������Ʈ ���·� �����ϴ� �ڷ�ƾ ����
            EditorCoroutineUtility.StartCoroutine(GenerateModel(animator, _dryRun), this);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Clear progress bar"))
            EditorUtility.ClearProgressBar();

    }


    /// <summary>�ִϸ��̼ǵ��� ���� ��ũ���ͺ� ������Ʈ ���·� �����Ű�� �Լ�</summary>
    private IEnumerator GenerateModel(Animator animator, bool dryRun)
    {

        string parentFolder = BASE_PATH + _name + "/";

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in _animatedModel.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            int clipIndex = 1;

            //��ũ���ͺ� ������Ʈ�� �����Ѵ�.
            AnimatedMeshScriptableObject scriptableObject = CreateInstance<AnimatedMeshScriptableObject>();
            scriptableObject.AnimationFPS = _animationFPS;

            //�ִϸ����Ϳ� ��ϵ� Ŭ������ �ݺ��Ѵ�.
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                //���� �ٸ� �����ش�.
                EditorUtility.DisplayProgressBar("Processing Animations", $"Processing animation {clip.name} ({clipIndex / animator.runtimeAnimatorController.animationClips.Length})", clipIndex / (float)animator.runtimeAnimatorController.animationClips.Length);
                AnimatedMeshScriptableObject.Animation animation = new AnimatedMeshScriptableObject.Animation();
                List<AnimatedMeshScriptableObject.MeshData> meshDatas = new List<AnimatedMeshScriptableObject.MeshData>();

                animation.Name = clip.name;
                float increment = 1f / _animationFPS;
                animator.Play(clip.name);

                //fps *  clip.length ��ŭ �ݺ��Ѵ�.
                for (float time = increment, length = clip.length; time < length; time += increment)
                {
                    //�ִϸ����͸� �������� �����Ѵ�.
                    animator.Update(increment);

                    if (_dryRun)
                        yield return new WaitForSeconds(increment);

                    AnimatedMeshScriptableObject.MeshData meshData = new AnimatedMeshScriptableObject.MeshData();
                    meshData.VertexList = new List<Vector3>();
                    meshData.NomalList = new List<Vector3>();

                    //���� ��Ų �޽� �������� ���¸� ������ �޽÷� ��ȯ
                    meshData.Mesh = new Mesh();
                    skinnedMeshRenderer.BakeMesh(meshData.Mesh);

                    if (_optimize)
                        meshData.Mesh.Optimize();

                    if (!_dryRun)
                    {
                        //���� �ش� ��ο� ������ �������� ������?
                        if (!AssetDatabase.IsValidFolder(parentFolder + clip.name + $"/{skinnedMeshRenderer.name}"))
                            System.IO.Directory.CreateDirectory(parentFolder + clip.name + $"/{skinnedMeshRenderer.name}");//������ �����Ѵ�.

                        AssetDatabase.CreateAsset(meshData.Mesh, parentFolder + clip.name + $"/{skinnedMeshRenderer.name}/{time:N4}.asset");
                    }

                    meshData.VertexList = meshData.Mesh.vertices.ToList();
                    meshData.NomalList = meshData.Mesh.normals.ToList();
                    meshDatas.Add( meshData );
                }

                Debug.Log($"Setting {clip.name} to have {meshDatas.Count} meshes");
                animation.MeshDataList = meshDatas;
                scriptableObject.Animations.Add(animation);
                clipIndex++;
            }

            if (!_dryRun)
            {
                Debug.Log($"Creating asset with {scriptableObject.Animations.Count} animations and {scriptableObject.Animations.Sum((item) => item.MeshDataList.Count)} meshes");
                EditorUtility.SetDirty(scriptableObject);
                AssetDatabase.CreateAsset(scriptableObject, BASE_PATH + skinnedMeshRenderer.name +" " + _name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

        }

        EditorUtility.ClearProgressBar();
    }


}
