using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

[CanEditMultipleObjects]
[CustomEditor(typeof(Gun), true)]
public class GunEditor : Editor
{
    private Gun _gun;

    private void OnEnable()
    {
        _gun = target as Gun;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("�ѱ� ����");
        EditorGUILayout.Space();
        _gun.Name = EditorGUILayout.TextField("�̸�", _gun.Name);
        _gun.Range = EditorGUILayout.FloatField("�����Ÿ�", _gun.Range);
        _gun.MinRecoil = EditorGUILayout.FloatField("���� �ݵ�", _gun.MinRecoil);
        _gun.MaxRecoil = EditorGUILayout.FloatField("�ִ� �ݵ�", _gun.MaxRecoil);
        _gun.Recoil = EditorGUILayout.Slider("�ѱ� �ݵ�", _gun.Recoil, 0.01f, 1f);
        _gun.RecoilRecoveryAmount = EditorGUILayout.Slider("�ݵ� ȸ����", _gun.RecoilRecoveryAmount, 0.1f, 1f);
        _gun.RPM = EditorGUILayout.FloatField("�д� �߻�ӵ�", _gun.RPM);
        _gun.ReloadTime = EditorGUILayout.FloatField("������ �ð�(S)", _gun.ReloadTime);

        EditorGUILayout.Space();
        _gun.Damage = EditorGUILayout.IntField("�ߴ� ������", _gun.Damage);

        EditorGUILayout.Space();
        _gun.MaxBulletCount = EditorGUILayout.IntField("źâ �ִ� �Ѿ� ����", _gun.MaxBulletCount);
        _gun.ReloadBulletCount = EditorGUILayout.IntField("������ �Ѿ� ����" , _gun.ReloadBulletCount);
        _gun.CarryBulletCount = EditorGUILayout.IntField("�κ��丮 �Ѿ� ����", _gun.CarryBulletCount);

        EditorGUILayout.Space();
        _gun.MuzzleFlash = (VisualEffect)EditorGUILayout.ObjectField("�ѱ� ȭ�� ����Ʈ", _gun.MuzzleFlash, typeof(VisualEffect), true);
        _gun.FireSound = (AudioClip)EditorGUILayout.ObjectField("�߻� ����", _gun.FireSound, typeof(AudioClip), true);
        _gun.ReloadSound = (AudioClip)EditorGUILayout.ObjectField("������ ����", _gun.ReloadSound, typeof(AudioClip), true);

        EditorGUILayout.LabelField("ź���: " + _gun.CurrentBulletCount);

        EditorUtility.SetDirty(_gun);
    }
}
