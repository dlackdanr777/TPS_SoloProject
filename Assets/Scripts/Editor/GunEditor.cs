using UnityEditor;
using UnityEngine;

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
        EditorGUILayout.LabelField("총기 설정");
        EditorGUILayout.Space();
        _gun.Name = EditorGUILayout.TextField("이름", _gun.Name);
        _gun.Range = EditorGUILayout.FloatField("사정거리", _gun.Range);
        _gun.Accuracy = EditorGUILayout.FloatField("정확도", _gun.Accuracy);
        _gun.RPM = EditorGUILayout.FloatField("분당 발사속도", _gun.RPM);
        _gun.ReloadTime = EditorGUILayout.FloatField("재장전 시간(S)", _gun.ReloadTime);

        EditorGUILayout.Space();
        _gun.Damage = EditorGUILayout.IntField("발당 데미지", _gun.Damage);

        EditorGUILayout.Space();
        _gun.MaxBulletCount = EditorGUILayout.IntField("탄창 최대 총알 수량", _gun.MaxBulletCount);
        _gun.ReloadBulletCount = EditorGUILayout.IntField("재장전 총알 수량" , _gun.ReloadBulletCount);
        _gun.CarryBulletCount = EditorGUILayout.IntField("인벤토리 총알 수량", _gun.CarryBulletCount);

        EditorGUILayout.Space();
        _gun.RetroActionForce = EditorGUILayout.FloatField("반동 세기", _gun.RetroActionForce);
        _gun.RetroActionFineSightForce = EditorGUILayout.FloatField("정조준 반동 세기", _gun.RetroActionFineSightForce);

        EditorGUILayout.Space();
        _gun.MuzzleFlash = (ParticleSystem)EditorGUILayout.ObjectField("총구 화염 이펙트", _gun.MuzzleFlash, typeof(ParticleSystem), true);
        _gun.FireSound = (AudioClip)EditorGUILayout.ObjectField("발사 사운드", _gun.FireSound, typeof(AudioClip), true);

        EditorGUILayout.LabelField("탄약수: " + _gun.CurrentBulletCount);

        EditorUtility.SetDirty(_gun);
    }
}
