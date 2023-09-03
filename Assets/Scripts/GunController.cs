using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun _currentGun; //현재 들고있는 총

    private Player _player;

    private float _currentFireRate; // 이값이 0이어야 총 발사 가능

    private AudioSource _audioSource;

    private bool _isReload = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        GunFireRateCalc();
        TryReload();
    }

    private void GunFireRateCalc()  
    {
        if(_currentFireRate > 0)
        {
            _currentFireRate -= Time.deltaTime;
        }
    }

    public void TryFire() //발사 입력을 받는 함수
    {
        if (Input.GetButton("Fire1") && _currentFireRate <= 0 && !_isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if(!_isReload)
        {
            if(_currentGun.CurrentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(ReloadRoutine());
            }
        }

    }

    public void Shoot()
    {
        _currentFireRate = _currentGun.FireRate;
        _currentGun.CurrentBulletCount--; //탄약 감소
        PlaySound(_currentGun.FireSound);
        //float verticalRand = Random.Range(0.2f, 0.5f); 
        //_player.PlayerCamera.CameraRotate(0, verticalRand);
        //_currentGun.MuzzleFlash.Emit(1);
        _currentGun.MuzzleFlash.Play();
        _player.MyAnimator.SetTrigger("Fire");
        Debug.Log("총알 발사");
    }

    public void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReload &&_currentGun.CurrentBulletCount < _currentGun.ReloadBulletCount)
        {

            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        if(_currentGun.CarryBulletCount > 0)
        {
            Debug.Log("재장전중");
            _isReload = true;
            _player.Machine.IsReload = true;
            _player.MyAnimator.SetTrigger("Reload");
            _currentGun.CarryBulletCount += _currentGun.CurrentBulletCount;
            _currentGun.CurrentBulletCount = 0;

            yield return new WaitForSeconds(_currentGun.ReloadTime);

            if(_currentGun.CarryBulletCount >= _currentGun.ReloadBulletCount)
            {
                _currentGun.CurrentBulletCount = _currentGun.ReloadBulletCount;
                _currentGun.CarryBulletCount -= _currentGun.ReloadBulletCount;
            }
            else
            {
                _currentGun.CurrentBulletCount = _currentGun.CarryBulletCount;
                _currentGun.CarryBulletCount = 0;
            }
            Debug.Log("재장전 끝");
            _isReload = false;
            _player.Machine.IsReload = false;
        }
        else
        {
            Debug.Log("보유한 총알수량이 부족합니다.");
        }
    }



    private void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.PlayOneShot(clip);
    }
}
