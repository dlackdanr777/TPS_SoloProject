using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string Name; //이름
    public float Range; //사정거리
    public float Accuracy; //정확도
    public float RPM; //분당 발사속도
    public float ReloadTime; //재장전 시간

    public int Damage; //총의 데미지

    public int MaxBulletCount; //총알의 최대 갯수
    public int ReloadBulletCount; //총알의 재장전 갯수
    public int CurrentBulletCount; //탄창에 남아있는 총알의 갯수 
    public int CarryBulletCount; //인벤토리의 총알 갯수

    public float RetroActionForce; //반동 세기
    public float RetroActionFineSightForce; //정조준의 반동 세기

    public Animator Anim;
    public ParticleSystem MuzzleFlash; //총구 화염 이펙트
    public AudioClip FireSound; //총 발사 소리

    public float FireRate; //총알 발사 간격

    public void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public void Start()
    {
        FireRate = 1 / (RPM / 60);
    }

}
